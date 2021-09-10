using Microsoft.Extensions.DependencyInjection;

namespace EndogenousUpdater.Configurations
{
    public interface IVersionBasedUpdatesBuilder
    {
        IServiceCollection Services { get; }
        IVersionBasedUpdatesTargetBuilder Source { get; }
        IVersionBasedUpdatesTargetBuilder Destination { get; }
    }

    public interface IVersionBasedUpdatesTargetBuilder
    {
        IServiceCollection Services { get; }
    }

    public class VersionBasedUpdatesBuilder : IVersionBasedUpdatesBuilder
    {
        public IServiceCollection Services { get; }
        public IVersionBasedUpdatesTargetBuilder Source { get; }
        public IVersionBasedUpdatesTargetBuilder Destination { get; }

        public VersionBasedUpdatesBuilder(IServiceCollection services)
        {
            this.Services = services;
            this.Source = new VersionBasedUpdatesTargetBuilder(services);
            this.Destination = new VersionBasedUpdatesTargetBuilder(services);
        }
    }

    public class VersionBasedUpdatesTargetBuilder : IVersionBasedUpdatesTargetBuilder
    {
        public IServiceCollection Services { get; }

        public VersionBasedUpdatesTargetBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
    }
}
