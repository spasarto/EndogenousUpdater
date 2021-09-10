namespace EndogenousUpdater.UpdateDestinations
{
    public class DirectoryUpdateDestination : IUpdateDestination
    {
        public string DirectoryPath { get; }

        public DirectoryUpdateDestination(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }
    }
}
