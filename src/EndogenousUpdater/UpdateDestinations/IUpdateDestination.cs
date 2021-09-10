
namespace EndogenousUpdater.UpdateDestinations
{
    /// <summary>
    /// Defines the update destination - where the update files should be copied to to successfully apply it.
    /// </summary>
    public interface IUpdateDestination
    {
        /// <summary>
        /// The directory path to update.
        /// </summary>
        string DirectoryPath { get; }
    }
}
