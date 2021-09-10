using Microsoft.Extensions.DependencyInjection;

namespace EndogenousUpdater.Configurations
{
    public interface IVersionBasedUpdatesBuilder
    {
        IServiceCollection Services { get; }
        ISourceVersionBasedUpdatesBuilder Source { get; }
        IDestinationVersionBasedUpdatesBuilder Destination { get; }
    }

    public interface ISourceVersionBasedUpdatesBuilder
    {
        IServiceCollection Services { get; }
    }

    public interface IDestinationVersionBasedUpdatesBuilder
    {
        IServiceCollection Services { get; }
    }

    public class VersionBasedUpdatesBuilder : IVersionBasedUpdatesBuilder
    {
        public IServiceCollection Services { get; }
        public ISourceVersionBasedUpdatesBuilder Source { get; }
        public IDestinationVersionBasedUpdatesBuilder Destination { get; }

        public VersionBasedUpdatesBuilder(IServiceCollection services)
        {
            this.Services = services;
            this.Source = new VersionBasedUpdatesTargetBuilder(services);
            this.Destination = new VersionBasedUpdatesTargetBuilder(services);
        }
    }

    public class VersionBasedUpdatesTargetBuilder : ISourceVersionBasedUpdatesBuilder, IDestinationVersionBasedUpdatesBuilder
    {
        public IServiceCollection Services { get; }

        public VersionBasedUpdatesTargetBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
    }
}
