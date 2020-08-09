using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Services;
using TidepoolToNightScoutSync.BL.Extensions;

using Xunit;
using TidepoolToNightScoutSync.BL.Services.Tidepool;
using Moq;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using TidepoolToNightScoutSync.BL.Model.Tidepool;
using FluentAssertions;
using System.Linq;

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
            client.Setup(x => x.GetBolusAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).Returns(FromFile<IReadOnlyList<Bolus>>("Data/bolus.json"));
            client.Setup(x => x.GetFoodAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).Returns(FromFile<IReadOnlyList<Food>>("Data/food.json"));
            client.Setup(x => x.GetPhysicalActivityAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>())).Returns(FromFile<IReadOnlyList<PhysicalActivity>>("Data/phys.json"));

            var factory = new Mock<ITidepoolClientFactory>();
            factory.Setup(x => x.CreateAsync()).Returns(Task.FromResult(client.Object));
            return factory.Object;
        }

        public BasicTest()
        {
            var configuration = new ConfigurationBuilder()
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

        [Fact]
        public async Task SyncAsync()
        {
            var boluses = await _tidepool.GetBolusAsync();
            var food = await _tidepool.GetFoodAsync();
            var activity = await _tidepool.GetPhysicalActivityAsync();
            var treatments = await _syncer.SyncAsync();

            boluses.All(x => treatments.Count(t => t.Insulin == x.Normal && t.CreatedAt == x.Time) == 1).Should().BeTrue();
            food.All(x => treatments.Count(t => t.Carbs == x.Nutrition?.Carbohydrate?.Net && t.CreatedAt == x.Time) == 1).Should().BeTrue();
            activity.All(x => treatments.Count(t => t.Notes == x.Name && t.Duration == x.Duration?.Value / 60 && t.CreatedAt == x.Time) == 1).Should().BeTrue();
        }
    }
}
