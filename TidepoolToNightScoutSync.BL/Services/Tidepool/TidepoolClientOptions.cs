﻿using Pathoschild.Http.Client;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Model.Tidepool;

namespace TidepoolToNightScoutSync.BL.Services.Tidepool
{
    public class TidepoolClientOptions
    {
        public string BaseUrl { get; set; } = "https://api.tidepool.org";
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
