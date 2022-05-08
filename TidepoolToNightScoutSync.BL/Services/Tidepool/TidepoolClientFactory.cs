using Microsoft.Extensions.Options;

using Pathoschild.Http.Client;

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Model.Tidepool;

namespace TidepoolToNightScoutSync.BL.Services.Tidepool
{
    public class TidepoolClientFactory : ITidepoolClientFactory
    {
        private readonly IClient _client;
        private readonly IOptionsMonitor<TidepoolClientOptions> _optionsMonitor;

        public TidepoolClientFactory(IOptionsMonitor<TidepoolClientOptions> options, HttpClient client)
        {
            _optionsMonitor = options;
            _client = new FluentClient(new Uri(Options.BaseUrl), client);
        }
        
        private TidepoolClientOptions Options => _optionsMonitor.CurrentValue;

        private async Task<ITidepoolClient> AuthorizeAsync()
        {
            var response = await _client
                .PostAsync("auth/login")
                .WithBasicAuthentication(Options.Username, Options.Password)
                .AsResponse();

            var token = response.Message.Headers.GetValues("x-tidepool-session-token").Single();
            _client.AddDefault(x => x.WithHeader("x-tidepool-session-token", token));

            var authResponse = await response.As<AuthResponse>();

            if(string.IsNullOrEmpty(Options.UserId))
                Options.UserId = authResponse.Userid;

            return new TidepoolClient(_client, new TidepoolClientOptions
            {
                BaseUrl = Options.BaseUrl,
                UserId = string.IsNullOrEmpty(Options.UserId) ? authResponse.Userid : Options.UserId,
                Username = Options.Username,
                Password = Options.Password
            });
        }

        public Task<ITidepoolClient> CreateAsync() =>
              AuthorizeAsync();
    }
}
