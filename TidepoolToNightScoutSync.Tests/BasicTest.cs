using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Services;
using TidepoolToNightScoutSync.BL.Extensions;

using Xunit;

namespace TidepoolToNightScoutSync.Tests
{
    public class BasicTest
    {
        private readonly TidepoolToNightScoutSyncer _syncer;

        public BasicTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<BasicTest>()
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddTidepoolClient((settings, configuration) => configuration.Bind(settings))
                .AddNightscoutClient((settings, configuration) => configuration.Bind(settings))
                .AddTidepoolToNightScoutSyncer((settings, configuration) => configuration.Bind(settings))
                .BuildServiceProvider();

            _syncer = services.GetRequiredService<TidepoolToNightScoutSyncer>();
        }

        [Fact]
        public async Task SyncBasal()
        {

        }
    }
}
