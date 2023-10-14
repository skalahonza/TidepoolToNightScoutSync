using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Nightscout
{
    public class Basal
    {
        [JsonProperty("time")] public string? Time { get; set; }

        [JsonProperty("value")] public string? Value { get; set; }

        [JsonProperty("timeAsSeconds")] public string? TimeAsSeconds { get; set; }
    }
}