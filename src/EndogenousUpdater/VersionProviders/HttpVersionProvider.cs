using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.VersionProviders
{
    public class HttpVersionProvider : IVersionProvider
    {
        private readonly string uri;
        private readonly HttpClient httpClient;

        public HttpVersionProvider(HttpClient httpClient, string uri)
        {
            this.uri = uri;
            this.httpClient = httpClient;
        }

        public async Task<Version> GetVersionAsync(CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(uri, cancellationToken);

            response.EnsureSuccessStatusCode();

            var version = await response.Content.ReadAsStringAsync();

            return new Version(version);
        }
    }
}
