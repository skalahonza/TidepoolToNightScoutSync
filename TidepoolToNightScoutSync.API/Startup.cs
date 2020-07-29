
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using TidepoolToNightScoutSync.API;

using TidepoolToNightScoutSync.BL.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]
namespace TidepoolToNightScoutSync.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder) =>
            builder.Services
                .AddTidepoolClient((settings, configuration) => configuration.GetSection("tidepool").Bind(settings))
                .AddNightscoutClient((settings, configuration) => configuration.GetSection("nightscout").Bind(settings))
                .AddTidepoolToNightScoutSyncer((settings, configuration) => configuration.GetSection("sync").Bind(settings));
    }
}
