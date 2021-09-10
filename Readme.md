# Introduction 
Performs application updates using the application itself as the updater.

![Nuget](https://img.shields.io/nuget/v/EndogenousUpdater)

# Getting Started
To add EndogenousUpdater to your project, simply register it in your service collection:

``` csharp
services.AddEndogenousUpdater(services, u =>
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
```

This will get you endogenous updater, using version based update checks with the provided update updateDirectoryPath. The client will compare its entry assembly version to the verison number of the most recent zip file in the format XXXX-1.2.3.4.zip.

Note, if you want to keep things simple, try this instead:

``` csharp
services.AddEndogenousUpdater(@"\\my\lan\update-source");
```

Then somewhere in your code, request an instance of ```IEndogenousUpdater``` and call ```ApplyUpdatesAsync```.

``` csharp
public class MyUpdater
{
    private readonly IEndogenousUpdater updater;

    public MyMigrator(IEndogenousUpdater updater)
    {
        this.updater = updater;
    }

    public async Task ApplyUpdatesAsync(CancellationToken cancellationToken)
    {
        await updater.ApplyUpdatesAsync(cancellationToken);
    }
}
```

If you have logging enabled, you should see something like this in your logs:

```
info: EndogenousUpdater.Updater[0]
      Cleaning up any old files from previous instance.
info: EndogenousUpdater.Updaters.NewerVersionUpdateChecker[0]
      Checking client for version
info: EndogenousUpdater.Updaters.NewerVersionUpdateChecker[0]
      Found client version: 0.9.0.0
info: EndogenousUpdater.Updaters.NewerVersionUpdateChecker[0]
      Checking server for version
info: EndogenousUpdater.Updaters.NewerVersionUpdateChecker[0]
      Found server version: 1.0.0.0
info: EndogenousUpdater.Updater[0]
      Updates are available. Applying them now.
info: EndogenousUpdater.Updater[0]
      Downloading update file.
info: EndogenousUpdater.Updater[0]
      Moving existing files to old format so they can be cleaned up later.
info: EndogenousUpdater.Updater[0]
      Extracting update files to destination.
info: EndogenousUpdater.Updater[0]
      Restarting application
info: EndogenousUpdater.ApplicationLaunchers.ProcessApplicationLauncher[0]
      Launching new instance of application: Simple.exe
```

# Why EndogenousUpdater
* Simple as you want to make it. No 3rd party installers, no restrictions on how you package and deploy your application.
* Full async/await support.
* Fully customizable. Dependency Injection based means you can overwrite any aspect of the project.
* Familiar code structure. Based on the Microsoft.Extensions.* projects. If you are already leveraging those packages in your project, this package will seem very familiar in it's setup.

# Build and Test
Clone the code and press build in Visual Studio. Or use dotnet build/test to get started.

# Contribute
Please create a pull request with details on what you are improving.