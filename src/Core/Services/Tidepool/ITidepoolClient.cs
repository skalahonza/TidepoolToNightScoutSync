using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TidepoolToNightScoutSync.Core.Model.Tidepool;

namespace TidepoolToNightScoutSync.Core.Services.Tidepool
{
    public interface ITidepoolClient
    {
        Task<IReadOnlyList<Bolus>> GetBolusAsync(DateTime? start = null, DateTime? end = null);
        Task<IReadOnlyList<Food>> GetFoodAsync(DateTime? start = null, DateTime? end = null);
        Task<IReadOnlyList<PhysicalActivity>> GetPhysicalActivityAsync(DateTime? start = null, DateTime? end = null);
        Task<IReadOnlyList<PumpSettings>> GetPumpSettingsAsync(DateTime? start = null, DateTime? end = null);
        Task<IReadOnlyList<BgValue>> GetBgValues(DateTime? start = null, DateTime? end = null);
    }
}