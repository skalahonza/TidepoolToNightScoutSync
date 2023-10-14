using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.BL.Model.Nightscout
{
    public class Sen
    {
        [JsonProperty("time")] public string? Time { get; set; }

        [JsonProperty("value")] public string? Value { get; set; }

        [JsonProperty("timeAsSeconds")] public string? TimeAsSeconds { get; set; }
    }
}