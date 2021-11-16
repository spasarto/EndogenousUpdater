using EndogenousUpdater.VersionProviders;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace EndogenousUpdater.Configurations
{
    public class ConfigureHttpZipVersionOptions : IConfigureOptions<VersionOptions>
    {
        private readonly HttpClient httpClient;
        private readonly string uri;

        public ConfigureHttpZipVersionOptions(HttpClient httpClient, string uri)
        {
            this.httpClient = httpClient;
            this.uri = uri;
        }

        public void Configure(VersionOptions options)
        {
            options.Source = new HttpVersionProvider(httpClient, uri);
        }
    }
}
