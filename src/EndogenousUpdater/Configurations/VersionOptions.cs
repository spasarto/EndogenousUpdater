using EndogenousUpdater.VersionProviders;

namespace EndogenousUpdater.Configurations
{
    public class VersionOptions
    {
        public IVersionProvider Source { get; set; }
        public IVersionProvider Destination { get; set; }
    }
}
