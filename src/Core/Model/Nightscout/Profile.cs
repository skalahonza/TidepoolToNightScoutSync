using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Nightscout
{
    public class Profile
    {
        [JsonProperty("_id")] public string? Id { get; set; }

        [JsonProperty("defaultProfile")] public string? DefaultProfile { get; set; }

        [JsonProperty("store")]
        public Dictionary<string, ProfileInfo> Store { get; set; } = new Dictionary<string, ProfileInfo>();

        [JsonProperty("startDate")] public DateTime? StartDate { get; set; }

        [JsonProperty("mills")] public string? Mills { get; set; }

        [JsonProperty("units")] public string? Units { get; set; }

        [JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }
    }
}