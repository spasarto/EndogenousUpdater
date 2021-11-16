using EndogenousUpdater.Configurations;
using EndogenousUpdater.Updaters;
using EndogenousUpdater.VersionProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
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
        public static IVersionBasedUpdatesTargetBuilder WithAssemblyVersion(this IVersionBasedUpdatesTargetBuilder builder, Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));

            builder.Services.Configure<VersionOptions>(options => options.Destination = new AssemblyVersionProvider(assembly));
            return builder;
        }

        /// <summary>
        /// Determines the source (server) version from the given file name. The file should take the format 'XXXX-1.2.3.4.anyextension'.
        /// </summary>
        public static IVersionBasedUpdatesTargetBuilder WithFilenameVersion(this IVersionBasedUpdatesTargetBuilder builder, string filePath)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            builder.Services.Configure<VersionOptions>(options => options.Source = new FilenameVersionProvider(filePath));
            return builder;
        }

        /// <summary>
        /// Determines the source (server) version from the given directory. The files with the last alphabetical name will be used. The file should take the format 'XXXX-1.2.3.4.anyextension'.
        /// </summary>
        public static IVersionBasedUpdatesTargetBuilder WithDirectoryVersion(this IVersionBasedUpdatesTargetBuilder builder, string directoryPath)
        {
            if (directoryPath == null) throw new ArgumentNullException(nameof(directoryPath));

            builder.Services.Configure<VersionOptions>(options => options.Source = new DirectoryVersionProvider(directoryPath));
            return builder;
        }

        /// <summary>
        /// Determines the source (server) version from the given http endpoint. The endpoint should be a GET endpoint that returns a version number.
        /// </summary>
        public static IVersionBasedUpdatesTargetBuilder WithHttpVersion(this IVersionBasedUpdatesTargetBuilder builder, string uri) => WithHttpVersion(builder, uri, httpClient => { });

        /// <summary>
        /// Determines the source (server) version from the given http endpoint. The endpoint should be a GET endpoint that returns a version number.
        /// </summary>
        public static IVersionBasedUpdatesTargetBuilder WithHttpVersion(this IVersionBasedUpdatesTargetBuilder builder, string uri, Action<HttpClient> configureClient)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            builder.Services.AddHttpClient<IConfigureOptions<VersionOptions>, ConfigureHttpZipVersionOptions>(httpClient =>
            {
                configureClient(httpClient);
                return new ConfigureHttpZipVersionOptions(httpClient, uri);
            });

            return builder;
        }

        /// <summary>
        /// Determines the source (server) version from the given http endpoint. The endpoint should be a GET endpoint that returns a version number.
        /// </summary>
        public static IVersionBasedUpdatesTargetBuilder WithHttpVersion(this IVersionBasedUpdatesTargetBuilder builder, string uri, Action<HttpClient, IServiceProvider> configureClient)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            builder.Services.AddHttpClient<IConfigureOptions<VersionOptions>, ConfigureHttpZipVersionOptions>((httpClient, sp) =>
            {
                configureClient(httpClient, sp);
                return new ConfigureHttpZipVersionOptions(httpClient, uri);
            });

            return builder;
        }
    }
}
