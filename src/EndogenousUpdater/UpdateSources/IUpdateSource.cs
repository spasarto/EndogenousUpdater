using EndogenousUpdater.Staging;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.UpdateSources
{
    public interface IUpdateSource
    {
        Task<IStagedUpdate> StageApplicationAsync(CancellationToken cancellationToken);
    }
}
