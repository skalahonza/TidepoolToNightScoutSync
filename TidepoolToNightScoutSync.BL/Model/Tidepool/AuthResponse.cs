using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace TidepoolToNightScoutSync.BL.Model.Tidepool
{
    public class AuthResponse
    {
        [JsonProperty("emailVerified")]
        public bool EmailVerified { get; set; }

        [JsonProperty("emails")]
        public List<string> Emails { get; set; }

        [JsonProperty("termsAccepted")]
        public DateTime TermsAccepted { get; set; }

        [JsonProperty("userid")]
        public string Userid { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
