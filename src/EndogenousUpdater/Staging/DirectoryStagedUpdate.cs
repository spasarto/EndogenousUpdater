using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.Staging
{
    public class DirectoryStagedUpdate : IStagedUpdate
    {
        private readonly string stagedFolder;
        private bool disposedValue;

        public DirectoryStagedUpdate(string stagedFolder, bool cleanupSourceFolder = true)
        {
            this.stagedFolder = stagedFolder;
            disposedValue = !cleanupSourceFolder;
        }

        public Task CopyToDestinationAsync(string destinationPath, CancellationToken cancellationToken)
        {
            var directory = new DirectoryInfo(stagedFolder);
            var files = directory.GetFiles("*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var destFile = file.FullName.Replace(stagedFolder, destinationPath);
                File.Move(file.FullName, destFile);
            }

            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Directory.Delete(stagedFolder, true);
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
