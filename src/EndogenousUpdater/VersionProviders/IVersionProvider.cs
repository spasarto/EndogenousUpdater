using System;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.VersionProviders
{
    public interface IVersionProvider
    {
        Task<Version> GetVersionAsync(CancellationToken cancellationToken);
    }
}
