using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Tidepool
{
    public class GlucoseTarget
    {
        [JsonProperty("start")] public int Start { get; set; }

        [JsonProperty("target")] public double Target { get; set; }
    }
}