using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pathoschild.Http.Client;
using TidepoolToNightScoutSync.Core.Extensions;
using TidepoolToNightScoutSync.Core.Model.Tidepool;

namespace TidepoolToNightScoutSync.Core.Services.Tidepool
{
    public class TidepoolClient : ITidepoolClient
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
                .WithArgument("startDate", start?.ToUniversalTime().ToString("o"))
                .WithArgument("endDate", end?.ToUniversalTime().ToString("o"))
                .WithArgument("type", nameof(DataType.Bolus).ToCamelCase())
                .AsArray<Bolus>();

        public async Task<IReadOnlyList<Food>> GetFoodAsync(DateTime? start = null, DateTime? end = null) =>
            await _client
                .GetAsync($"data/{_options.UserId}")
                .WithArgument("startDate", start?.ToUniversalTime().ToString("o"))
                .WithArgument("endDate", end?.ToUniversalTime().ToString("o"))
                .WithArgument("type", nameof(DataType.Food).ToCamelCase())
                .AsArray<Food>();

        public async Task<IReadOnlyList<PhysicalActivity>> GetPhysicalActivityAsync(DateTime? start = null,
            DateTime? end = null) =>
            await _client
                .GetAsync($"data/{_options.UserId}")
                .WithArgument("startDate", start?.ToUniversalTime().ToString("o"))
                .WithArgument("endDate", end?.ToUniversalTime().ToString("o"))
                .WithArgument("type", nameof(DataType.PhysicalActivity).ToCamelCase())
                .AsArray<PhysicalActivity>();

        public async Task<IReadOnlyList<PumpSettings>> GetPumpSettingsAsync(DateTime? start = null,
            DateTime? end = null) =>
            await _client
                .GetAsync($"data/{_options.UserId}")
                .WithArgument("startDate", start?.ToUniversalTime().ToString("o"))
                .WithArgument("endDate", end?.ToUniversalTime().ToString("o"))
                .WithArgument("type", nameof(DataType.PumpSettings).ToCamelCase())
                .AsArray<PumpSettings>();

        public async Task<IReadOnlyList<BgValue>> GetBgValues(DateTime? start = null,
            DateTime? end = null) =>
            await _client
                .GetAsync($"data/{_options.UserId}")
                .WithArgument("startDate", start?.ToUniversalTime().ToString("o"))
                .WithArgument("endDate", end?.ToUniversalTime().ToString("o"))
                .WithArgument("type", nameof(DataType.Cbg).ToLower())
                .AsArray<BgValue>();
    }
}