using Microsoft.Extensions.Options;

using Pathoschild.Http.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Model.Nightscout;

namespace TidepoolToNightScoutSync.BL.Services.Nightscout
{
    public class NightscoutClient
    {
        private readonly IClient _client;
        private readonly IOptionsMonitor<NightscoutClientOptions> _optionsMonitor;

        public NightscoutClient(IOptionsMonitor<NightscoutClientOptions> options, HttpClient client)
        {
            _optionsMonitor = options;
            _client = new FluentClient(new Uri(Options.BaseUrl), client);
            _client.AddDefault(x => x.WithArgument("token", Options.ApiKey));
            _client.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        }

        private NightscoutClientOptions Options => _optionsMonitor.CurrentValue;

        private static string SHA1(in string input)
        {
            using var sha1 = new SHA1Managed();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Concat(hash.Select(b => b.ToString("x2")));
        }

        public async Task<IReadOnlyList<Profile>> GetProfiles() =>
            await _client
                .GetAsync("api/v1/profile")
                .AsArray<Profile>();

        public async Task<Profile> SetProfile(Profile profile) =>
            await _client
               .PutAsync("api/v1/profile", profile)
               .WithHeader("api-secret", SHA1(Options.ApiKey))
               .As<Profile>();

        public async Task<IReadOnlyList<Treatment>> AddTreatmentsAsync(IEnumerable<Treatment> treatments) =>
            await _client
                .PostAsync("api/v1/treatments", treatments)
                .WithHeader("api-secret", SHA1(Options.ApiKey))
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
