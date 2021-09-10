using EndogenousUpdater.Staging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.UpdateSources
{
    public class ZipFileUpdateSource : IUpdateSource
    {
        private readonly string fileName;

        public ZipFileUpdateSource(string fileName)
        {
            this.fileName = fileName;
        }

        public async Task<IStagedUpdate> StageApplicationAsync(CancellationToken cancellationToken)
        {
            var tempFile = Path.GetTempFileName();
            var fileStream = File.Open(tempFile, FileMode.OpenOrCreate);
            using (var sourceStream = File.Open(fileName, FileMode.Open))
            {
                await sourceStream.CopyToAsync(fileStream, 81920, cancellationToken);
            }

            return new ZipFileStagedUpdate(fileStream);
        }
    }
}
