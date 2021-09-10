using EndogenousUpdater.UpdateDestinations;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.Cleaners
{
    public interface IExistingFilesCleaner
    {
        Task CleanupOldFilesAsync(CancellationToken cancellationToken);
        Task MarkForCleanupAsync(CancellationToken cancellationToken);
    }

    public class ExistingFilesCleaner : IExistingFilesCleaner
    {
        private readonly string directoryPath;
        private readonly ILogger<ExistingFilesCleaner> logger;

        public string PidFilePath => Path.Combine(directoryPath, nameof(EndogenousUpdater) + ".pid.old");

        public ExistingFilesCleaner(IUpdateDestination updateDestination, ILogger<ExistingFilesCleaner> logger)
        {
            this.directoryPath = updateDestination.DirectoryPath ?? throw new ArgumentNullException(nameof(updateDestination), "directoryPath is not set on update destination");
            this.logger = logger;
        }

        public Task CleanupOldFilesAsync(CancellationToken cancellationToken)
        {
            var destinationDirectory = new DirectoryInfo(directoryPath);
            if(!destinationDirectory.Exists)
                destinationDirectory.Create();

            TryWaitForOldProcess();

            foreach (var file in destinationDirectory.GetFiles("*.old", SearchOption.AllDirectories))
            {
                try
                {
                    file.Delete();
                }
                catch { } // files may still be use if the process hasn't died yet.
            }
            return Task.CompletedTask;
        }

        private void TryWaitForOldProcess()
        {
            if (File.Exists(PidFilePath) && int.TryParse(File.ReadAllText(PidFilePath), out int pid))
            {
                logger.LogInformation("Lock file exists from existing process. Checking for running process.");
                try
                {
                    var process = Process.GetProcessById(pid);
                    if (process != null)
                    {
                        logger.LogInformation("Process {pid} found. Waiting for it to exit normally.", pid);
                        if (!process.WaitForExit(250))
                        {
                            logger.LogInformation("Process did not respond in a timely manner. Killing it.");
                            process.Kill();
                        }
                    }
                }
                catch { }
            }
        }

        public Task MarkForCleanupAsync(CancellationToken cancellationToken)
        {
            var destinationDirectory = new DirectoryInfo(directoryPath);

            foreach (var file in destinationDirectory.GetFiles("*", SearchOption.AllDirectories))
            {
                file.MoveTo(file.FullName + ".old");
            }

#if NET5_0_OR_GREATER
            var pid = System.Environment.ProcessId;
#else
            var pid = Process.GetCurrentProcess().Id;
#endif
            File.WriteAllText(PidFilePath, pid.ToString());

            return Task.CompletedTask;
        }
    }


}
