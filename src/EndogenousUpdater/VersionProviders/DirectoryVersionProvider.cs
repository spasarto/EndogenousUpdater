using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.VersionProviders
{
    public class DirectoryVersionProvider : IVersionProvider
    {
        private readonly string directoryPath;

        public DirectoryVersionProvider(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public Task<Version> GetVersionAsync(CancellationToken cancellationToken)
        {
            var directory = new DirectoryInfo(directoryPath);

            if (!directory.Exists)
                throw new ArgumentException($"Directory '{directoryPath}' does not exist. Cannot infer version from it.");

            var version = directory.GetFiles("*.zip")
                                   .OrderByDescending(v => v.Name)
                                   .Select(f => new FilenameVersionProvider(f.Name))
                                   .FirstOrDefault();

            return version.GetVersionAsync(cancellationToken);
        }
    }
}
