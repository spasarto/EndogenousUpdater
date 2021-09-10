using EndogenousUpdater.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.Updaters
{
    public class NewerVersionUpdateChecker : IUpdateChecker
    {
        private readonly ILogger<NewerVersionUpdateChecker> logger;
        private readonly VersionOptions versionOptions;

        public NewerVersionUpdateChecker(IOptions<VersionOptions> versionOptions,
                                         ILogger<NewerVersionUpdateChecker> logger)
        {
            this.versionOptions = versionOptions.Value;
            this.logger = logger;
        }

        public async Task<bool> ShouldApplyUpdateAsync(CancellationToken cancellationToken)
        {
            var clientVersion = await GetClientVersionAsync(cancellationToken);

            var serverVersion = await GetServerVersionAsync(cancellationToken);

            return clientVersion < serverVersion;
        }

        private async Task<Version> GetServerVersionAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Checking server for version");
            var serverVersion = await versionOptions.Source.GetVersionAsync(cancellationToken);
            logger.LogInformation("Found server version: {serverVersion}", serverVersion);
            return serverVersion;
        }

        private async Task<Version> GetClientVersionAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Checking client for version");
            var clientVersion = await versionOptions.Destination.GetVersionAsync(cancellationToken);
            logger.LogInformation("Found client version: {clientVersion}", clientVersion);
            return clientVersion;
        }
    }
}
