using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Model.Tidepool;

namespace TidepoolToNightScoutSync.BL.Services.Tidepool
{
    public interface ITidepoolClient
    {
        Task<IReadOnlyList<Bolus>> GetBolusAsync(DateTime? start = null, DateTime? end = null);
        Task<IReadOnlyList<Food>> GetFoodAsync(DateTime? start = null, DateTime? end = null);
        Task<IReadOnlyList<PhysicalActivity>> GetPhysicalActivityAsync(DateTime? start = null, DateTime? end = null);
    }
}