using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Nightscout
{
    public class ProfileInfo
    {
        [JsonProperty("dia")] public string? Dia { get; set; }

        [JsonProperty("carbratio")] public List<Carbratio> Carbratio { get; } = new List<Carbratio>();

        [JsonProperty("carbs_hr")] public string? CarbsHr { get; set; }

        [JsonProperty("delay")] public string? Delay { get; set; }

        [JsonProperty("sens")] public List<Sen> Sens { get; } = new List<Sen>();

        [JsonProperty("timezone")] public string? Timezone { get; set; }

        [JsonProperty("basal")] public List<Basal> Basal { get; } = new List<Basal>();

        [JsonProperty("target_low")] public List<Target> TargetLow { get; } = new List<Target>();

        [JsonProperty("target_high")] public List<Target> TargetHigh { get; } = new List<Target>();

        [JsonProperty("startDate")] public DateTime? StartDate { get; set; }

        [JsonProperty("units")] public string? Units { get; set; }
    }
}