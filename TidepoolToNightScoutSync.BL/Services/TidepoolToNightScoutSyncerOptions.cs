using System;

namespace TidepoolToNightScoutSync.BL.Services
{
    public class TidepoolToNightScoutSyncerOptions
    {
        public DateTime? Since { get; set; }
        public DateTime? Till { get; set; }
    }
}
