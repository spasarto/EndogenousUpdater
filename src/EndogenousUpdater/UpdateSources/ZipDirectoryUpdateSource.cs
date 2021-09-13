using EndogenousUpdater.Staging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.UpdateSources
{
    public class ZipDirectoryUpdateSource : IUpdateSource
    {
        private readonly string directoryPath;

        public ZipDirectoryUpdateSource(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public Task<IStagedUpdate> StageApplicationAsync(CancellationToken cancellationToken)
        {
            var directory = new DirectoryInfo(directoryPath);
            var updateSource = directory.GetFiles("*.zip")
                                   .OrderByDescending(v => v.FullName)
                                   .Select(f => new ZipFileUpdateSource(f.FullName))
                                   .FirstOrDefault();

            return updateSource.StageApplicationAsync(cancellationToken);
        }
    }
}
