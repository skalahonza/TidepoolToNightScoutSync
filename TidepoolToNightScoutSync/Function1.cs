using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using TidepoolToNightScoutSync.BL.Model.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Tidepool;

namespace TidepoolToNightScoutSync
{
    public class Function1
    {
        private readonly TidepoolClientFactory _factory;
        private readonly NightscoutClient _nightscout;

        public Function1(TidepoolClientFactory factory, NightscoutClient nightscout)
        {
            _factory = factory;
            _nightscout = nightscout;
        }


        private async Task Sync()
        {
            var tidepool = await _factory.CreateAsync();
            var boluses = (await tidepool.GetBolusAsync(DateTime.Today)).ToDictionary(x => x.Time, x => x);
            var food = (await tidepool.GetFoodAsync(DateTime.Today)).ToDictionary(x => x.Time, x => x);
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
                    Carbs = x?.Nutrition?.Carbohydrate?.Net,
                    CreatedAt = x.Time,
                    EnteredBy = "Tidepool"
                }));
            var response = await _nightscout.AddTreatmentsAsync(treatments);
        }

        [FunctionName("Function2")]
        public async Task Run2([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req, ILogger log)
        {
            await Sync();
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {

        }
    }
}
