using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Tidepool
{
    public class InsulinSensitivity
    {
        [JsonProperty("amount")] public double Amount { get; set; }

        [JsonProperty("start")] public int Start { get; set; }
    }
}