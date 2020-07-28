using Microsoft.Extensions.Options;

using Pathoschild.Http.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Model.Tidepool;

namespace TidepoolToNightScoutSync.BL.Services.Tidepool
{
    public class TidepoolClient
    {
        private readonly IClient _client;
        private readonly TidepoolClientOptions _options;

        internal TidepoolClient(IClient client, TidepoolClientOptions options)
        {
            _client = client;
            _options = options;
        }

        public async Task<IReadOnlyList<Bolus>> GetBolusAsync(DateTime? start = null, DateTime? end = null) =>
            await _client
                .GetAsync($"data/{_options.UserId}")
                .WithArgument("startDate", start?.ToString("o"))
                .WithArgument("endDate", end?.ToString("o"))
                .WithArgument("type", nameof(DataType.Bolus).ToLower())
                .AsArray<Bolus>();

        public async Task<IReadOnlyList<Food>> GetFoodAsync(DateTime? start = null, DateTime? end = null) =>
            await _client
                .GetAsync($"data/{_options.UserId}")
                .WithArgument("startDate", start?.ToString("o"))
                .WithArgument("endDate", end?.ToString("o"))
                .WithArgument("type", nameof(DataType.Food).ToLower())
                .AsArray<Food>();
    }
}
