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

        public Task<IStagedUpdate> StageApplicationAsync(CancellationToken cancellationToken)
        {
            var tempFile = Path.GetTempFileName();
            File.Copy(fileName, tempFile, true);

            return Task.FromResult<IStagedUpdate>(new ZipFileStagedUpdate(File.OpenRead(tempFile)));
        }
    }
}
