using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class Carbohydrate
    {
        [JsonProperty("net")] public double? Net { get; set; }

        [JsonProperty("units")] public string? Units { get; set; }
    }
}