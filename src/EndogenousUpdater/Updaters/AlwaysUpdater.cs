using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.Updaters
{
    public class AlwaysUpdater : IUpdateChecker
    {
        public Task<bool> ShouldApplyUpdateAsync(CancellationToken cancellationToken) => Task.FromResult(true);
    }
}
