using Microsoft.Extensions.DependencyInjection;

namespace EndogenousUpdater.Configurations
{
    public interface IUpdaterBuilder
    {
        IServiceCollection Services { get; }
    }

    public class UpdaterBuilder : IUpdaterBuilder
    {
        public IServiceCollection Services { get; }

        public UpdaterBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
    }
}
