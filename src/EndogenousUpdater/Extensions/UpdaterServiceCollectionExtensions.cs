using EndogenousUpdater.ApplicationLaunchers;
using EndogenousUpdater.Cleaners;
using EndogenousUpdater.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace EndogenousUpdater
{
    public static class UpdaterServiceCollectionExtensions
    {
        /// <summary>
        /// Adds EndogenousUpdater, using version based update checks with the provided update DirectoryPath. The client will compare its entry assembly version to the verison number of the most recent zip file in the format XXXX-1.2.3.4.zip.
        /// </summary>
        /// <param name="updateDirectoryPath">A directory that contains your updates, in the form of zip archives. The version number will be obtained from the zipe file names.</param>
        public static IServiceCollection AddEndogenousUpdater(this IServiceCollection services, string updateDirectoryPath)
            => AddEndogenousUpdater(services, u =>
                {
                    u.WithVersionBasedUpdates(v =>
                    {
                        v.Source.WithDirectoryVersion(updateDirectoryPath);
                        v.Destination.WithAssemblyVersion(Assembly.GetEntryAssembly());
                    });
                    u.ZipDirectorySource(updateDirectoryPath); 
                    u.CurrentDirectoryDestination();
                    u.WithRelaunchOptions(o => o.InferLaunchOptions());
                });

        /// <summary>
        /// Adds EndogenousUpdater with the provided configuration.
        /// </summary>
        public static IServiceCollection AddEndogenousUpdater(this IServiceCollection services, Action<IUpdaterBuilder> configure)
        {
            services.AddTransient<IEndogenousUpdater, Updater>();
            services.AddTransient<IExistingFilesCleaner, ExistingFilesCleaner>();
            services.AddTransient<IApplicationLauncher, ProcessApplicationLauncher>();
            services.AddLogging();

            var builder = new UpdaterBuilder(services);

            configure(builder);

            return services;
        }
    }
}
