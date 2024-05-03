using System;
using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Tidepool
{
    public class PhysicalActivity
    {
        [JsonProperty("distance")] public Distance? Distance { get; set; }

        [JsonProperty("duration")] public Duration? Duration { get; set; }

        [JsonProperty("energy")] public Energy? Energy { get; set; }

        [JsonProperty("id")] public string? Id { get; set; }

        [JsonProperty("name")] public string? Name { get; set; }

        [JsonProperty("time")] public DateTime? Time { get; set; }

        [JsonProperty("uploadId")] public string? UploadId { get; set; }
    }
}