using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.VersionProviders
{
    public class FilenameVersionProvider : IVersionProvider
    {
        private readonly string fileName;

        public Regex VersionRegex { get; set; } = new Regex(@"^.*?[\.-]((?:\d\.)+\d)$");

        public FilenameVersionProvider(string fileName)
        {
            this.fileName = fileName;
        }

        public Task<Version> GetVersionAsync(CancellationToken cancellationToken)
        {
            var file = Path.GetFileNameWithoutExtension(fileName);

            var match = this.VersionRegex.Match(file);

            var version = match.Success ? new Version(match.Groups[1].Value) : default;

            return Task.FromResult(version);
        }
    }
}
