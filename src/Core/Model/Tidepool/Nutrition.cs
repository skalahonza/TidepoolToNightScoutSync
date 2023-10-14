using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Tidepool
{
    public class Nutrition
    {
        [JsonProperty("carbohydrate")] public Carbohydrate Carbohydrate { get; set; }
    }
}