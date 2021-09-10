using EndogenousUpdater.Staging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.UpdateSources
{
    public class DirectoryUpdateSource : IUpdateSource
    {
        private readonly string directoryPath;

        public DirectoryUpdateSource(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public async Task<IStagedUpdate> StageApplicationAsync(CancellationToken cancellationToken)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            var stager = new DirectoryStagedUpdate(directoryPath, false);
            await stager.CopyToDestinationAsync(tempDirectory, cancellationToken);

            Directory.CreateDirectory(tempDirectory);

            return new DirectoryStagedUpdate(tempDirectory);
        }
    }
}
