using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class Carbohydrate
    {

        [JsonProperty("net")]
        public int Net { get; set; }

        [JsonProperty("units")]
        public string Units { get; set; }

    }
}
