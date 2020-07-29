using Microsoft.Extensions.Options;

using Pathoschild.Http.Client;

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using TidepoolToNightScoutSync.BL.Model.Tidepool;

namespace TidepoolToNightScoutSync.BL.Services.Tidepool
{
    public class TidepoolClientFactory
    {
        private readonly IClient _client;
        private readonly TidepoolClientOptions _options;

        public TidepoolClientFactory(IOptions<TidepoolClientOptions> options, HttpClient client)
        {
            _options = options.Value;
            _client = new FluentClient(new Uri(_options.BaseUrl), client);
        }

        private async Task<TidepoolClient> AuthorizeAsync()
        {
            var response = await _client
                .PostAsync("/auth/login")
                .WithBasicAuthentication(_options.Username, _options.Password)
                .AsResponse();

            var token = response.Message.Headers.GetValues("x-tidepool-session-token").Single();
            _client.AddDefault(x => x.WithHeader("x-tidepool-session-token", token));

            var authResponse = await response.As<AuthResponse>();
            _options.UserId = authResponse.Userid;

            return new TidepoolClient(_client, _options);
        }

        public Task<TidepoolClient> CreateAsync() =>
              AuthorizeAsync();
    }
}
