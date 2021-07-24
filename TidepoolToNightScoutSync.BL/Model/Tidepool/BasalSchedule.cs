using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class BasalSchedule
    {
        [JsonProperty("rate")]
        public double Rate { get; set; }

        [JsonProperty("start")]
        public int Start { get; set; }        
    }
}
