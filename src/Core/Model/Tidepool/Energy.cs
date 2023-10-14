using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class Energy
    {
        [JsonProperty("units")] public string Units { get; set; }

        [JsonProperty("value")] public double Value { get; set; }
    }
}