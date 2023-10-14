using System;
using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Nightscout
{
    public class Treatment
    {
        [JsonProperty("_id")] public string? Id { get; set; }

        [JsonProperty("eventType")] public string? EventType { get; set; }

        [JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }

        [JsonProperty("glucose")] public string? Glucose { get; set; }

        [JsonProperty("glucoseType")] public string? GlucoseType { get; set; }

        [JsonProperty("carbs")] public double? Carbs { get; set; }

        [JsonProperty("protein")] public double? Protein { get; set; }

        [JsonProperty("fat")] public double? Fat { get; set; }

        [JsonProperty("insulin")] public double? Insulin { get; set; }

        [JsonProperty("relative")] public double? Relative { get; set; }

        [JsonProperty("units")] public string Units { get; set; } = "mmol";

        [JsonProperty("notes")] public string? Notes { get; set; }

        [JsonProperty("enteredBy")] public string? EnteredBy { get; set; }

        [JsonProperty("duration")] public double? Duration { get; set; }
    }
}