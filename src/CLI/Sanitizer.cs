using System.Web;
using Serilog.Core;
using Serilog.Events;

namespace TidepoolToNightScoutSync.CLI
{
    public class Sanitizer : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Properties.TryGetValue("Uri", out var value))
            {
                var builder = new UriBuilder(value.ToString());
                var parameters = HttpUtility.ParseQueryString(builder.Query);

                // sanitize token query parameter
                if (!string.IsNullOrEmpty(parameters.Get("token")))
                {
                    parameters.Set("token", "*****");
                    builder.Query = parameters.ToString();
                    logEvent.AddOrUpdateProperty(new LogEventProperty("Uri", new ScalarValue(builder.ToString())));
                }
            }
        }
    }
}