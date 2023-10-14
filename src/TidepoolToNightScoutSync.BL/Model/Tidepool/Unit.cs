using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class Unit
    {
        [JsonProperty("bg")] public string? Bg { get; set; }

        [JsonProperty("carb")] public string? Carb { get; set; }
    }
}