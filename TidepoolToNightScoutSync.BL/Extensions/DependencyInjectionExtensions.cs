using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TidepoolToNightScoutSync.BL.Services.Tidepool;

namespace TidepoolToNightScoutSync.BL.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTidepoolClient(this IServiceCollection services)
        {
            services.AddHttpClient<TidepoolClientFactory>();
            return services;
        }
    }
}
