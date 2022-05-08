using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Desktop.ViewModels.Credentials;

public class NightscoutCredentialsViewModel
{
    [JsonProperty("BaseUrl")] public string BaseUrl { get; set; } = "";

    [JsonProperty("ApiKey")] public string ApiKey { get; set; } = "";
}