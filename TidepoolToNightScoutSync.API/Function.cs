using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Pathoschild.Http.Client;

using TidepoolToNightScoutSync.BL.Services;

namespace TidepoolToNightScoutSync.API
{
    public class Function
    {
        private readonly TidepoolToNightScoutSyncer _syncer;
        private readonly ILogger _log;

        public Function(TidepoolToNightScoutSyncer syncer, ILogger<Function> log)
        {
            _syncer = syncer;
            _log = log;
        }

        private static DateTime? TryParse(string text) =>
            DateTime.TryParse(text, out var date) ? date : (DateTime?)null;

        private async Task ExecuteSafeAsync(Task task)
        {
            try
            {
                await task;
            }
            catch (ApiException ex)
            {
                _log.LogError(ex, await ex.Response.AsString());
                throw;
            }
        }

        [FunctionName(nameof(Timer))]
        public Task Timer([TimerTrigger("%Interval%")] TimerInfo timer) =>
            ExecuteSafeAsync(_syncer.SyncAsync());

        [FunctionName(nameof(Sync))]
        public async Task Sync([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            req.Query.TryGetValue("since", out var sinceValue);
            req.Query.TryGetValue("till", out var tillValue);
            var since = TryParse(sinceValue);
            var till = TryParse(tillValue);
            await ExecuteSafeAsync(_syncer.SyncProfiles(since, till));
            await ExecuteSafeAsync(_syncer.SyncAsync(since, till));
        }
    }
}
