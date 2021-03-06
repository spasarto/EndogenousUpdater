using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.Staging
{
    public class ZipFileStagedUpdate : IStagedUpdate
    {
        private readonly FileStream stream;
        private bool disposedValue;

        public ZipFileStagedUpdate(FileStream stream)
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
                    string filePath = stream.Name;
                    stream.Dispose();
                    File.Delete(filePath);
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
