using System;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.Staging
{
    /// <summary>
    /// Represents an update that has been stagged locally and is ready to be applied to the live folder.
    /// </summary>
    public interface IStagedUpdate : IDisposable
    {
        /// <summary>
        /// Copies the staged files to the live folder to apply the update.
        /// </summary>
        Task CopyToDestinationAsync(string destinationPath, CancellationToken cancellationToken);
    }
}
