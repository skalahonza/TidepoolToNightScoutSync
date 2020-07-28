using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class Nutrition
    {
        [JsonProperty("carbohydrate")]
        public Carbohydrate Carbohydrate { get; set; }
    }
}
