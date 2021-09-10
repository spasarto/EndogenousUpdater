using EndogenousUpdater.Configurations;
using EndogenousUpdater.UpdateDestinations;
using EndogenousUpdater.Updaters;
using EndogenousUpdater.UpdateSources;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EndogenousUpdater
{
    public static class UpdaterBuilderExtensions
    {
        /// <summary>
        /// Configures the updater to compare version numbers between the client and the server to determine if there are updates.
        /// </summary>
        public static IUpdaterBuilder WithVersionBasedUpdates(this IUpdaterBuilder builder, Action<IVersionBasedUpdatesBuilder> configure)
        {
            var vBuilder = new VersionBasedUpdatesBuilder(builder.Services);

            vBuilder.ApplyWhenNewerVersion();
            configure(vBuilder);

            return builder;
        }

        public static IUpdaterBuilder AlwaysUpdate(this IUpdaterBuilder builder)
        {
            builder.Services.AddTransient<IUpdateChecker, AlwaysUpdater>();
            return builder;
        }

        /// <summary>
        /// Configures the updater to pull its update from the given file. 
        /// You need to configure how the updater determines updates using the other extension methods.
        /// </summary>
        public static IUpdaterBuilder ZipFileSource(this IUpdaterBuilder builder, string fileName)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));

            builder.Services.AddSingleton<IUpdateSource, ZipFileUpdateSource>(sp => new ZipFileUpdateSource(fileName));
            return builder;
        }

        /// <summary>
        /// Configures the updater to pull its update from the given directory. It will search the directory for the zip file with the last alphabetical name and use that as the latest deployment. 
        /// You need to configure how the updater determines updates using the other extension methods.
        /// </summary>
        public static IUpdaterBuilder ZipDirectorySource(this IUpdaterBuilder builder, string directoryPath)
        {
            if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));

            builder.Services.AddSingleton<IUpdateSource, ZipDirectoryUpdateSource>(sp => new ZipDirectoryUpdateSource(directoryPath));
            return builder;
        }

        /// <summary>
        /// Configures the updater to push its updates to the provided assembly. 
        /// You need to configure how the updater determines updates using the other extension methods.
        /// </summary>
        public static IUpdaterBuilder AssemblyDestination(this IUpdaterBuilder builder, Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            builder.Services.AddSingleton<IUpdateDestination, AssemblyUpdateDestination>(sp => new AssemblyUpdateDestination(assembly));
            return builder;
        }

        /// <summary>
        /// Configures the updater to push its updates to the provided assembly. 
        /// You need to configure how the updater determines updates using the other extension methods.
        /// </summary>
        public static IUpdaterBuilder CurrentDirectoryDestination(this IUpdaterBuilder builder)
        {
            builder.Services.AddSingleton<IUpdateDestination, DirectoryUpdateDestination>(sp => new DirectoryUpdateDestination(Environment.CurrentDirectory));
            return builder;
        }

        public static IUpdaterBuilder DirectoryDestination(this IUpdaterBuilder builder, string directory)
        {
            builder.Services.AddSingleton<IUpdateDestination, DirectoryUpdateDestination>(sp => new DirectoryUpdateDestination(directory));
            return builder;
        }

        /// <summary>
        /// Configures the updater to relaunch the application after updates have been applied.
        /// </summary>
        public static IUpdaterBuilder WithRelaunchOptions(this IUpdaterBuilder builder, Action<ApplicationRestartOptions> configure)
        {
            builder.Services.Configure<ApplicationRestartOptions>(configure);
            return builder;
        }

        /// <summary>
        /// Attempts to infer the launch options based on how the current executing version was launched.
        /// </summary>
        public static ApplicationRestartOptions InferLaunchOptions(this ApplicationRestartOptions options, string directory = null)
        {
            var exe = Environment.GetCommandLineArgs()[0];
            string args;

            if (exe.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                args = Environment.CommandLine.Replace(exe, "");
            }
            else
            {
#if NET6_0
                exe = Environment.ProcessPath;
#else
                exe = Process.GetCurrentProcess().MainModule.FileName;
#endif
                args = string.Join(" ", Environment.GetCommandLineArgs().Skip(1));
            }

            if (directory != null)
            {
                var file = Path.GetFileName(exe);
                exe = Path.Combine(directory, file);
            }

            options.ProcessToRelaunchArguments = new ProcessStartInfo(exe, args);

            if(directory != null)
                options.ProcessToRelaunchArguments.WorkingDirectory = directory;

            return options;
        }

        /// <summary>
        /// Configures the updater to push its updates to the entry assembly. 
        /// You need to configure how the updater determines updates using the other extension methods.
        /// </summary>
        public static IUpdaterBuilder EntryAssemblyDestination(this IUpdaterBuilder builder) => builder.AssemblyDestination(Assembly.GetEntryAssembly());
    }
}
