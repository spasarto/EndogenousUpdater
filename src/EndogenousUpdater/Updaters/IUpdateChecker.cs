using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.Updaters
{
    public interface IUpdateChecker
    {
        Task<bool> ShouldApplyUpdateAsync(CancellationToken cancellationToken);
    }
}
