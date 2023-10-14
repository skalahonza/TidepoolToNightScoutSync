using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Core.Model.Tidepool
{
    public class PumpSettings
    {
        [JsonProperty("activeSchedule")] public string? ActiveSchedule { get; set; }

        [JsonProperty("automatedDelivery")] public bool AutomatedDelivery { get; set; }

        [JsonProperty("deviceTime")] public DateTime? DeviceTime { get; set; }

        [JsonProperty("basalSchedules")]
        public IReadOnlyDictionary<string, IReadOnlyList<BasalSchedule>> BasalSchedules { get; set; } =
            new Dictionary<string, IReadOnlyList<BasalSchedule>>();

        [JsonProperty("bgTargets")]
        public IReadOnlyDictionary<string, IReadOnlyList<GlucoseTarget>> BgTargets { get; set; } =
            new Dictionary<string, IReadOnlyList<GlucoseTarget>>();

        [JsonProperty("carbRatios")]
        public IReadOnlyDictionary<string, IReadOnlyList<CarbRatio>> CarbRatios { get; set; } =
            new Dictionary<string, IReadOnlyList<CarbRatio>>();

        [JsonProperty("insulinSensitivities")]
        public IReadOnlyDictionary<string, IReadOnlyList<InsulinSensitivity>> InsulinSensitivities { get; set; } =
            new Dictionary<string, IReadOnlyList<InsulinSensitivity>>();

        [JsonProperty("units")] public Unit Units { get; set; } = new Unit();
    }
}