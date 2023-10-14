using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TidepoolToNightScoutSync.BL.Services;
using TidepoolToNightScoutSync.BL.Services.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Tidepool;

namespace TidepoolToNightScoutSync.BL.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTidepoolToNightScoutSyncer(this IServiceCollection services,
            Action<TidepoolToNightScoutSyncerOptions, IConfiguration> configureOptions)
        {
            services.AddOptions<TidepoolToNightScoutSyncerOptions>().Configure(configureOptions);
            return services.AddSingleton<TidepoolToNightScoutSyncer>();
        }

        public static IServiceCollection AddNightscoutClient(this IServiceCollection services,
            Action<NightscoutClientOptions, IConfiguration> configureOptions)
        {
            services.AddHttpClient<NightscoutClient>();
            services.AddOptions<NightscoutClientOptions>().Configure(configureOptions);
            return services;
        }

        public static IServiceCollection AddTidepoolClient(this IServiceCollection services,
            Action<TidepoolClientOptions, IConfiguration> configureOptions)
        {
            services.AddHttpClient<ITidepoolClientFactory, TidepoolClientFactory>();
            services.AddOptions<TidepoolClientOptions>().Configure(configureOptions);
            return services;
        }
    }
}