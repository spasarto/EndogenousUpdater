using EndogenousUpdater.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.ApplicationLaunchers
{
    public interface IApplicationLauncher
    {
        Task RestartApplicationAsync(CancellationToken cancellationToken);
    }

    public class ProcessApplicationLauncher : IApplicationLauncher
    {
        private readonly ApplicationRestartOptions applicationRestartOptions;
        private readonly ILogger<ProcessApplicationLauncher> logger;

        public ProcessApplicationLauncher(IOptions<ApplicationRestartOptions> applicationRestartOptions, ILogger<ProcessApplicationLauncher> logger)
        {
            this.applicationRestartOptions = applicationRestartOptions.Value;
            this.logger = logger;
        }

        public Task RestartApplicationAsync(CancellationToken cancellationToken)
        {
            var psi = applicationRestartOptions.ProcessToRelaunchArguments;

            if (psi == null)
                logger.LogInformation("Application will not be re-launched as it was not configured to be.");
            else
                logger.LogInformation("Launching new instance of application: {exe} {args}", psi.FileName, psi.Arguments);

            Process.Start(psi);

            Environment.Exit(Environment.ExitCode);
            return Task.CompletedTask;
        }
    }
}
