using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TidepoolToNightScoutSync;
using TidepoolToNightScoutSync.BL.Extensions;
using TidepoolToNightScoutSync.BL.Services.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Tidepool;

[assembly: FunctionsStartup(typeof(Startup))]
namespace TidepoolToNightScoutSync
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTidepoolClient();
            
            // https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
            builder.Services
                .AddOptions<TidepoolClientOptions>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("tidepool").Bind(settings));

            builder.Services.AddNightscoutClient();
            builder.Services
                .AddOptions<NightscoutClientOptions>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("nightscout").Bind(settings));
        }
    }
}
