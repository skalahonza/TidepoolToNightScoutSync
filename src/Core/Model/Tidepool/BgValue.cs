using System;
using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Tidepool
{
    public class BgValue
    {
        [JsonProperty("clockDriftOffset")] public int? ClockDriftOffset { get; set; }

        [JsonProperty("conversionOffset")] public int? ConversionOffset { get; set; }

        [JsonProperty("deviceId")] public string DeviceId { get; set; }

        [JsonProperty("deviceTime")] public DateTime? DeviceTime { get; set; }

        [JsonProperty("guid")] public string Guid { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("time")] public DateTime? Time { get; set; }

        [JsonProperty("units")] public string Units { get; set; }

        [JsonProperty("value")] public double Value { get; set; }

        [JsonProperty("uploadId")] public string? UploadId { get; set; }
    }
}