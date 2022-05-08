using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Desktop.ViewModels.Credentials;

public class CredentialsViewModel
{
    [JsonProperty("tidepool")] public TidepoolCredentialsViewModel Tidepool { get; set; } = new();

    [JsonProperty("nightscout")] public NightscoutCredentialsViewModel Nightscout { get; set; } = new();
}