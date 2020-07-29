using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Model.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Nightscout;
using TidepoolToNightScoutSync.BL.Services.Tidepool;

namespace TidepoolToNightScoutSync.BL.Services
{
    public class TidepoolToNightScoutSyncerOptions
    {
        public DateTime? Since { get; set; }
        public DateTime? Till { get; set; }
    }
}
