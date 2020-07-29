using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Model.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Tidepool;

namespace TidepoolToNightScoutSync.BL.Services
{
    public class TidepoolToNightScoutSyncer
    {
        private readonly TidepoolClientFactory _factory;
        private readonly NightscoutClient _nightscout;
        private readonly TidepoolToNightScoutSyncerOptions _options;
        private TidepoolClient? tidepool;

        public TidepoolToNightScoutSyncer(TidepoolClientFactory factory, NightscoutClient nightscout, IOptions<TidepoolToNightScoutSyncerOptions> options)
        {
            _factory = factory;
            _nightscout = nightscout;
            _options = options.Value;
        }

        public async Task<IReadOnlyList<Treatment>> SyncAsync(DateTime? since = null, DateTime? till = null)
        {
            since ??= _options.Since;
            till ??= _options.Till;
            tidepool ??= await _factory.CreateAsync();
            var boluses = (await tidepool.GetBolusAsync(since, till))
                .GroupBy(x => x.Time)
                .Select(x => x.First())
                .ToDictionary(x => x.Time, x => x);
            var food = (await tidepool.GetFoodAsync(since, till))
                .GroupBy(x => x.Time)
                .Select(x => x.First())
                .ToDictionary(x => x.Time, x => x);
            var treatments = boluses
                .Values
                // standalone boluses and boluses with food
                .Select(x => new Treatment
                {
                    Carbs = food.GetValueOrDefault(x.Time)?.Nutrition?.Carbohydrate?.Net,
                    Insulin = x.Normal,
                    CreatedAt = x.Time,
                    EnteredBy = "Tidepool"
                })
                // food without boluses
                .Concat(food.Values.Where(x => !boluses.ContainsKey(x.Time)).Select(x => new Treatment
                {
                    Carbs = x.Nutrition?.Carbohydrate?.Net,
                    CreatedAt = x.Time,
                    EnteredBy = "Tidepool"
                }));
            return await _nightscout.AddTreatmentsAsync(treatments);
        }
    }
}
