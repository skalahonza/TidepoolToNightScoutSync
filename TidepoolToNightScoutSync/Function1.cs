using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using TidepoolToNightScoutSync.BL.Services.Tidepool;

namespace TidepoolToNightScoutSync
{
    public  class Function1
    {
        private readonly TidepoolClientFactory _factory;

        public Function1(TidepoolClientFactory factory)
        {
            _factory = factory;
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var client = await _factory.CreateAsync();
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            log.LogInformation($"{nameof(myTimer.ScheduleStatus.Last)}: {myTimer.ScheduleStatus.Last}");
            log.LogInformation($"{nameof(myTimer.ScheduleStatus.LastUpdated)}: {myTimer.ScheduleStatus.LastUpdated}");
            log.LogInformation($"{nameof(myTimer.ScheduleStatus.Next)}: {myTimer.ScheduleStatus.Next}");
            log.LogInformation(JsonConvert.SerializeObject(myTimer));
            var boluses = await client.GetBoluses();
            log.LogInformation(JsonConvert.SerializeObject(boluses));
        }
    }
}
