using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace EndogenousUpdater.VersionProviders
{
    public class AssemblyVersionProvider : IVersionProvider
    {
        private readonly Assembly assembly;

        public AssemblyVersionProvider(Assembly assembly)
        {
            this.assembly = assembly;
        }

        public Task<Version> GetVersionAsync(CancellationToken cancellationToken)
            => Task.FromResult(assembly.GetName().Version);
    }
}
