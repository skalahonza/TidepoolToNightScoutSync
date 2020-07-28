using Microsoft.Extensions.DependencyInjection;

using TidepoolToNightScoutSync.BL.Services.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Tidepool;

namespace TidepoolToNightScoutSync.BL.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddNightscoutClient(this IServiceCollection services)
        {
            services.AddHttpClient<NightscoutClient>();
            return services;
        }

        public static IServiceCollection AddTidepoolClient(this IServiceCollection services)
        {
            services.AddHttpClient<TidepoolClientFactory>();
            return services;
        }
    }
}
