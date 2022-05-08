using Newtonsoft.Json;

namespace TidepoolToNightScoutSync.Desktop.ViewModels.Credentials;

public class TidepoolCredentialsViewModel
{
    [JsonProperty("Username")] public string Username { get; set; } = "";

    [JsonProperty("Password")] public string Password { get; set; } = "";
}