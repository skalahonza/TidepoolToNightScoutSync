using System;
using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Tidepool
{
    public class Bolus
    {
        [JsonProperty("id")] public string? Id { get; set; }

        [JsonProperty("normal")] public double? Normal { get; set; }

        [JsonProperty("extended")] public double? Extended { get; set; }

        /// <summary>
        /// Duration in milliseconds.
        /// </summary>
        [JsonProperty("duration")]
        public long? DurationMs { get; set; }

        [JsonProperty("subType")] public string? SubType { get; set; }

        [JsonProperty("time")] public DateTime? Time { get; set; }

        [JsonProperty("uploadId")] public string? UploadId { get; set; }

        public TimeSpan? Duration => DurationMs.HasValue
            ? TimeSpan.FromMilliseconds(DurationMs.Value)
            : default(TimeSpan?);
    }
}