using EndogenousUpdater.ApplicationLaunchers;
using EndogenousUpdater.Cleaners;
using EndogenousUpdater.Staging;
using EndogenousUpdater.UpdateDestinations;
using EndogenousUpdater.Updaters;
using EndogenousUpdater.UpdateSources;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater
{
    public interface IEndogenousUpdater
    {
        Task ApplyUpdatesAsync(CancellationToken cancellationToken);
    }

    public class Updater : IEndogenousUpdater
    {
        private readonly IUpdateSource updateSource;
        private readonly IUpdateDestination updateDestination;
        private readonly IExistingFilesCleaner existingFilesCleaner;
        private readonly IUpdateChecker updateChecker;
        private readonly IApplicationLauncher applicationLauncher;
        private readonly ILogger logger;

        public Updater(IUpdateSource updateSource,
                       IUpdateDestination updateDestination,
                       IExistingFilesCleaner existingFilesCleaner,
                       IUpdateChecker updateChecker,
                       IApplicationLauncher applicationLauncher,
                       ILogger<Updater> logger)
        {
            this.updateSource = updateSource;
            this.updateDestination = updateDestination;
            this.existingFilesCleaner = existingFilesCleaner;
            this.updateChecker = updateChecker;
            this.applicationLauncher = applicationLauncher;
            this.logger = logger;
        }

        public async Task ApplyUpdatesAsync(CancellationToken cancellationToken)
        {
            await CleanupOldFilesAsync(cancellationToken);

            var applyUpdates = await updateChecker.ShouldApplyUpdateAsync(cancellationToken);

            if (!applyUpdates)
            {
                logger.LogInformation($"Not applying updates as client version matches or exceeds server version.");
                return;
            }

            logger.LogInformation("Updates are available. Applying them now.");
            using (var updateFile = await DownloadUpdateAsync(cancellationToken))
            {
                await MoveExistingFilesAsync(cancellationToken);

                await ExtractUpdateAsync(updateFile, cancellationToken);
            }

            logger.LogInformation("Restarting application");
            await applicationLauncher.RestartApplicationAsync(cancellationToken);
        }

        private async Task ExtractUpdateAsync(IStagedUpdate stagedUpdate, CancellationToken cancellationToken)
        {
            logger.LogInformation("Extracting update files to destination.");
            await stagedUpdate.CopyToDestinationAsync(updateDestination.DirectoryPath, cancellationToken);
        }

        private async Task MoveExistingFilesAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Moving existing files to old format so they can be cleaned up later.");
            await existingFilesCleaner.MarkForCleanupAsync(cancellationToken); ;
        }

        private async Task CleanupOldFilesAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Cleaning up any old files from previous instance.");
            await existingFilesCleaner.CleanupOldFilesAsync(cancellationToken);
        }

        private async Task<IStagedUpdate> DownloadUpdateAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Downloading update file.");
            return await updateSource.StageApplicationAsync(cancellationToken);
        }
    }
}
