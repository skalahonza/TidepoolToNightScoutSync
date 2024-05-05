using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using TidepoolToNightScoutSync.Core.Extensions;
using TidepoolToNightScoutSync.Core.Model.Tidepool;
using TidepoolToNightScoutSync.Core.Services;
using TidepoolToNightScoutSync.Core.Services.Tidepool;
using Xunit;

namespace TidepoolToNightScoutSync.Tests
{
    public class BasicTest
    {
        private readonly TidepoolToNightScoutSyncer _syncer;
        private readonly ITidepoolClient _tidepool;

        private static async Task<T> FromFile<T>(string path) =>
            JsonConvert.DeserializeObject<T>(await File.ReadAllTextAsync(path));

        private ITidepoolClientFactory MockFactory()
        {
            var client = new Mock<ITidepoolClient>();
            client.Setup(x => x.GetBolusAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns(FromFile<IReadOnlyList<Bolus>>("Data/bolus.json"));
            client.Setup(x => x.GetFoodAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns(FromFile<IReadOnlyList<Food>>("Data/food.json"));
            client.Setup(x => x.GetPhysicalActivityAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns(FromFile<IReadOnlyList<PhysicalActivity>>("Data/phys.json"));
            client.Setup(x => x.GetPumpSettingsAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns(FromFile<IReadOnlyList<PumpSettings>>("Data/pumpSettings.json"));
            client.Setup(x => x.GetBgValues(It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .Returns(FromFile<IReadOnlyList<BgValue>>("Data/bgValues.json"));

            var factory = new Mock<ITidepoolClientFactory>();
            factory.Setup(x => x.CreateAsync()).Returns(Task.FromResult(client.Object));
            return factory.Object;
        }

        public BasicTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets<BasicTest>()
                .Build();

            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton(MockFactory())
                .AddNightscoutClient((settings, configuration) => configuration.GetSection("nightscout").Bind(settings))
                .AddTidepoolToNightScoutSyncer((settings, configuration) => configuration.Bind(settings))
                .BuildServiceProvider();

            _syncer = services.GetRequiredService<TidepoolToNightScoutSyncer>();
            _tidepool = services.GetRequiredService<ITidepoolClientFactory>().CreateAsync().Result;
        }

        private bool AreEqualApproximately(double left, double right, double v) =>
            Math.Abs(left - right) < v;

        [Fact]
        public async Task SyncProfiles()
        {
            // Arrange
            var nfi = new CultureInfo("en-US", false).NumberFormat;
            var settings = await _tidepool.GetPumpSettingsAsync();
            var setting = settings.OrderByDescending(x => x.DeviceTime).FirstOrDefault();

            // Act
            var profile = await _syncer.SyncProfiles();

            // Assert
            profile.Store.Keys.Should().Contain(setting.BasalSchedules.Select(x => x.Key));
            profile.Store.Keys.Should().Contain(setting.BgTargets.Select(x => x.Key));
            profile.Store.Keys.Should().Contain(setting.CarbRatios.Select(x => x.Key));
            profile.Store.Keys.Should().Contain(setting.InsulinSensitivities.Select(x => x.Key));

            // basal
            foreach (var (name, schedule) in setting.BasalSchedules.Select(x => (x.Key, x.Value)))
            {
                var expectedBasal = schedule.Select(x => ((x.Start / 1000).ToString(nfi), x.Rate.ToString(nfi)));
                profile.Store[name].Basal
                    .Select(x => (x.TimeAsSeconds, x.Value))
                    .Should().BeEquivalentTo(expectedBasal);
            }

            // bg targets 
            foreach (var (name, targets) in setting.BgTargets.Select(x => (x.Key, x.Value)))
            {
                var expectedTimes = targets.Select(x => (x.Start / 1000).ToString(nfi));
                var expectedTargets = targets.Select(x => x.Target);

                profile.Store[name].TargetLow.Select(x => x.TimeAsSeconds).Should()
                    .BeEquivalentTo(profile.Store[name].TargetHigh.Select(x => x.TimeAsSeconds));
                profile.Store[name].TargetLow.Select(x => x.Time).Should()
                    .BeEquivalentTo(profile.Store[name].TargetHigh.Select(x => x.Time));
                profile.Store[name].TargetLow.Select(x => x.TimeAsSeconds).Should().BeEquivalentTo(expectedTimes);


                // interval middle value whould be equal to Tidepool target
                profile.Store[name].TargetLow.Zip(profile.Store[name].TargetHigh)
                    .Select(x =>
                        double.Parse(x.Second.Value, CultureInfo.InvariantCulture) -
                        double.Parse(x.First.Value, CultureInfo.InvariantCulture))
                    .Should()
                    .Equal(expectedTargets, (left, right) => AreEqualApproximately(left, right, 0.001));
            }

            // carb ratios
            foreach (var (name, carbRatios) in setting.CarbRatios.Select(x => (x.Key, x.Value)))
            {
                var expectedRatios = carbRatios.Select(x => ((x.Start / 1000).ToString(nfi), x.Amount.ToString(nfi)));
                profile.Store[name].Carbratio
                    .Select(x => (x.TimeAsSeconds, x.Value))
                    .Should().BeEquivalentTo(expectedRatios);
            }

            // insulin sensitivities
            foreach (var (name, sensitivities) in setting.InsulinSensitivities.Select(x => (x.Key, x.Value)))
            {
                var expectedSensitivities =
                    sensitivities.Select(x => ((x.Start / 1000).ToString(nfi), x.Amount.ToString(nfi)));
                profile.Store[name].Sens
                    .Select(x => (x.TimeAsSeconds, x.Value))
                    .Should().BeEquivalentTo(expectedSensitivities);
            }
        }

        [Fact]
        public async Task SyncAsync()
        {
            // Arrange
            var boluses = await _tidepool.GetBolusAsync();
            var food = await _tidepool.GetFoodAsync();
            var activities = await _tidepool.GetPhysicalActivityAsync();
            var bgValues = await _tidepool.GetBgValues();

            // Act
            var treatments = await _syncer.SyncAsync();

            // Assert

            // Boluses
            foreach (var bolus in boluses)
            {
                treatments.Should().ContainSingle(
                    t => t.Insulin == bolus.Normal && t.CreatedAt == bolus.Time,
                    because: "Every bolus should be synced exactly once.");
            }

            // Food
            foreach (var item in food)
            {
                treatments.Should().ContainSingle(
                    t => t.Carbs == item.Nutrition.Carbohydrate.Net && t.CreatedAt == item.Time,
                    because: "Every food should be synced exactly once.");
            }

            // Activities
            foreach (var activity in activities)
            {
                treatments.Should().ContainSingle(
                    t => t.Notes == activity.Name && t.Duration == activity.Duration.Value / 60 &&
                         t.CreatedAt == activity.Time,
                    because: "Every activity should be synced exactly once.");
            }

            // BG Values
            foreach (var bgValue in bgValues)
            {
                treatments.Should().ContainSingle(
                    t => t.Glucose == bgValue.Value.ToString(CultureInfo.InvariantCulture) &&
                         t.CreatedAt == bgValue.Time,
                    because: "Every BG value should be synced exactly once.");
            }
        }
    }
}