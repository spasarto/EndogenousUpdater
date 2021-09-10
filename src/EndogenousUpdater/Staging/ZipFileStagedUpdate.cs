using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.Staging
{
    public class ZipFileStagedUpdate : IStagedUpdate
    {
        private readonly Stream stream;
        private bool disposedValue;

        public ZipFileStagedUpdate(Stream stream)
        {
            this.stream = stream;
        }

        public Task CopyToDestinationAsync(string destinationPath, CancellationToken cancellationToken)
        {
            var archive = new ZipArchive(stream);

            archive.ExtractToDirectory(destinationPath);

            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    stream.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
