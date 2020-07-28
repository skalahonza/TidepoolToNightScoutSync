using Newtonsoft.Json;

using System;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class Bolus
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("normal")]
        public double? Normal { get; set; }

        [JsonProperty("subType")]
        public string? SubType { get; set; }

        [JsonProperty("time")]
        public DateTime? Time { get; set; }

        [JsonProperty("uploadId")]
        public string? UploadId { get; set; }
    }
}
