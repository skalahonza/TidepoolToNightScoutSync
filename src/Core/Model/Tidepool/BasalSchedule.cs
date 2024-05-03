using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Tidepool
{
    public class BasalSchedule
    {
        [JsonProperty("rate")] public double Rate { get; set; }

        [JsonProperty("start")] public int Start { get; set; }
    }
}