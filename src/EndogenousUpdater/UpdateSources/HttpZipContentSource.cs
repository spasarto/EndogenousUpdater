using EndogenousUpdater.Staging;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.UpdateSources
{
    public class HttpZipContentSource : IUpdateSource
    {
        private readonly HttpClient httpClient;
        private readonly string uri;

        public HttpZipContentSource(HttpClient httpClient, string uri)
        {
            this.httpClient = httpClient;
            this.uri = uri;
        }

        public async Task<IStagedUpdate> StageApplicationAsync(CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(uri, cancellationToken);

            response.EnsureSuccessStatusCode();

            var tempFile = Path.GetTempFileName();

            var fileStream = File.Open(tempFile, FileMode.OpenOrCreate);

            await response.Content.CopyToAsync(fileStream);

            fileStream.Position = 0;

            return new ZipFileStagedUpdate(fileStream);
        }
    }
}
