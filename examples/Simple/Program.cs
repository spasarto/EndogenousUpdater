using EndogenousUpdater;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Simple
{
    public static class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Press any key to apply updates");
            var key = Console.ReadKey();

            try
            {
                var services = new ServiceCollection();
                services.AddLogging(c =>
                {
                    c.AddConsole();
                });

                const string destinationDirectory = @"C:\ProgramData\Company\Simple";
                if (string.Equals(destinationDirectory, Environment.CurrentDirectory))
                {
                    services.AddEndogenousUpdater(@"F:\Source\Repos\EndogenousUpdater\examples\Simple\update-source");
                }
                else
                {
                    // "installer" option
                    services.AddEndogenousUpdater(u =>
                    {
                        u.ZipFileSource(@"F:\Source\Repos\EndogenousUpdater\examples\Simple\update-source\simple-1.0.0.0.zip");
                        u.DirectoryDestination(destinationDirectory);
                        u.AlwaysUpdate();
                        u.WithRelaunchOptions(o => o.InferLaunchOptions(destinationDirectory));
                    });
                }

                var serviceProvider = services.BuildServiceProvider();
                var updater = serviceProvider.GetRequiredService<IEndogenousUpdater>();

                await updater.ApplyUpdatesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().Name);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}