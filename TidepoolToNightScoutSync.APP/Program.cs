using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Extensions;
using TidepoolToNightScoutSync.BL.Services;

namespace TidepoolToNightScoutSync.APP
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            //logging
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // dependency injection
            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddTidepoolClient((settings, configuration) => configuration.GetSection("tidepool").Bind(settings))
                .AddNightscoutClient((settings, configuration) => configuration.GetSection("nightscout").Bind(settings))
                .AddTidepoolToNightScoutSyncer((settings, configuration) => configuration.GetSection("sync").Bind(settings))
                .AddLogging(x => x.AddSerilog())
                .BuildServiceProvider();

            var syncer = services.GetRequiredService<TidepoolToNightScoutSyncer>();
            await syncer.SyncAsync();
        }
    }
}
