using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.BL.Model.Nightscout
{
    public class Treatment
    {
        [JsonProperty("_id")]
        public string? Id { get; set; }

        [JsonProperty("eventType")]
        public string? EventType { get; set; }

        [JsonProperty("created_at")]
        public string? CreatedAt { get; set; }

        [JsonProperty("glucose")]
        public string? Glucose { get; set; }

        [JsonProperty("glucoseType")]
        public string? GlucoseType { get; set; }

        [JsonProperty("carbs")]
        public int? Carbs { get; set; }

        [JsonProperty("protein")]
        public int? Protein { get; set; }

        [JsonProperty("fat")]
        public int? Fat { get; set; }

        [JsonProperty("insulin")]
        public int? Insulin { get; set; }

        [JsonProperty("units")]
        public string? Units { get; set; }

        [JsonProperty("notes")]
        public string? Notes { get; set; }

        [JsonProperty("enteredBy")]
        public string? EnteredBy { get; set; }
    }
}
