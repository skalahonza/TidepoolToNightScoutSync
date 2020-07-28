using Microsoft.Extensions.Options;

using Pathoschild.Http.Client;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
                .PostAsync("treatments", treatments)
                .AsArray<Treatment>();
    }
}
