namespace TidepoolToNightScoutSync.Core.Services.Tidepool
{
    public class TidepoolClientOptions
    {
        public string BaseUrl { get; set; } = "https://api.tidepool.org";
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}