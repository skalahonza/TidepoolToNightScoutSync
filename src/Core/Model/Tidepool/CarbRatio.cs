using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class CarbRatio
    {
        [JsonProperty("amount")] public double Amount { get; set; }

        [JsonProperty("start")] public int Start { get; set; }
    }
}