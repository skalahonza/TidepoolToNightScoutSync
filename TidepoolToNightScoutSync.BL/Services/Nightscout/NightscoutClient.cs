using Microsoft.Extensions.Options;

using Pathoschild.Http.Client;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Model.Nightscout;

namespace TidepoolToNightScoutSync.BL.Services.Nightscout
{
    public class NightscoutClient
    {
        private readonly IClient _client;
        private readonly NightscoutClientOptions _options;

        public NightscoutClient(IOptions<NightscoutClientOptions> options, HttpClient client)
        {
            _options = options.Value;
            _client = new FluentClient(new Uri(_options.BaseUrl), client);
            _client.AddDefault(x => x.WithArgument("token", _options.ApiKey));
            _client.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        }

        public async Task<IReadOnlyList<Treatment>> AddTreatmentsAsync(IEnumerable<Treatment> treatments) =>
            await _client
                .PostAsync("api/v1/treatments", treatments)
                .AsArray<Treatment>();

        /// <summary>
        /// Get information about the Nightscout treatments.
        /// </summary>
        /// <param name="find">The query used to find entries, supports nested query syntax. Examples find[insulin][$gte]=3 find[carb][$gte]=100 find[eventType]=Correction+Bolus All find parameters are interpreted as strings.</param>
        /// <param name="count">Number of entries to return.</param>
        /// <returns></returns>
        public async Task<IReadOnlyList<Treatment>> GetTreatmentsAsync(string? find, int? count) =>
            await _client
                .GetAsync("api/v1/treatments")
                .WithArgument("find", find)
                .WithArgument("count", count)
                .AsArray<Treatment>();

        
    }
}
