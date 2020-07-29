using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class Nutrition
    {
        [JsonProperty("carbohydrate")]
        public Carbohydrate Carbohydrate { get; set; }
    }
}
