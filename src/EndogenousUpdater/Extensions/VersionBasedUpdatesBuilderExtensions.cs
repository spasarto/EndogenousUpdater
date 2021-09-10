using EndogenousUpdater.Configurations;
using EndogenousUpdater.Updaters;
using EndogenousUpdater.VersionProviders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace EndogenousUpdater
{
    public static class VersionBasedUpdatesBuilderExtensions
    {
        /// <summary>
        /// Applies updates when the client version is behind the server version. This is the default behavior, and this doesn't need to be called directly in most cases.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IVersionBasedUpdatesBuilder ApplyWhenNewerVersion(this IVersionBasedUpdatesBuilder builder)
        {
            builder.Services.AddTransient<IUpdateChecker, NewerVersionUpdateChecker>();
            return builder;
        }

        /// <summary>
        /// Determines the destination (client) version from the provided assembly.
        /// </summary>
        public static IDestinationVersionBasedUpdatesBuilder WithAssemblyVersion(this IDestinationVersionBasedUpdatesBuilder builder, Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            builder.Services.Configure<VersionOptions>(options => options.Destination = new AssemblyVersionProvider(assembly));
            return builder;
        }

        /// <summary>
        /// Determines the source (server) version from the given file name. The file should take the format 'XXXX-1.2.3.4.anyextension'.
        /// </summary>
        public static ISourceVersionBasedUpdatesBuilder WithFilenameVersion(this ISourceVersionBasedUpdatesBuilder builder, string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            builder.Services.Configure<VersionOptions>(options => options.Source = new FilenameVersionProvider(filePath));
            return builder;
        }

        /// <summary>
        /// Determines the source (server) version from the given directory. The files with the last alphabetical name will be used. The file should take the format 'XXXX-1.2.3.4.anyextension'.
        /// </summary>
        public static ISourceVersionBasedUpdatesBuilder WithDirectoryVersion(this ISourceVersionBasedUpdatesBuilder builder, string directoryPath)
        {
            if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));

            builder.Services.Configure<VersionOptions>(options => options.Source = new DirectoryVersionProvider(directoryPath));
            return builder;
        }
    }
}
