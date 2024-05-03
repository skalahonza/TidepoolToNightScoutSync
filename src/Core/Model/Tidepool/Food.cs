using System;
using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Tidepool
{
    public class Food
    {
        [JsonProperty("id")] public string? Id { get; set; }

        [JsonProperty("nutrition")] public Nutrition? Nutrition { get; set; }

        [JsonProperty("time")] public DateTime? Time { get; set; }

        [JsonProperty("uploadId")] public string? UploadId { get; set; }
    }
}