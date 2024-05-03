using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pathoschild.Http.Client;
using TidepoolToNightScoutSync.Core.Model.Tidepool;

namespace TidepoolToNightScoutSync.Core.Services.Tidepool
{
    public class TidepoolClientFactory : ITidepoolClientFactory
    {
        private readonly IClient _client;
        private readonly TidepoolClientOptions _options;

        public TidepoolClientFactory(IOptions<TidepoolClientOptions> options, HttpClient client)
        {
            _options = options.Value;
            _client = new FluentClient(new Uri(_options.BaseUrl), client);
        }

        private async Task<ITidepoolClient> AuthorizeAsync()
        {
            var response = await _client
                .PostAsync("auth/login")
                .WithBasicAuthentication(_options.Username, _options.Password)
                .AsResponse();

            var token = response.Message.Headers.GetValues("x-tidepool-session-token").Single();
            _client.AddDefault(x => x.WithHeader("x-tidepool-session-token", token));

            var authResponse = await response.As<AuthResponse>();

            if (string.IsNullOrEmpty(_options.UserId))
                _options.UserId = authResponse.Userid;

            return new TidepoolClient(_client, _options);
        }

        public Task<ITidepoolClient> CreateAsync() =>
            AuthorizeAsync();
    }
}