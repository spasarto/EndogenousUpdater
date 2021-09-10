using System;
using System.IO;
using System.Reflection;

namespace EndogenousUpdater.UpdateDestinations
{
    public class AssemblyUpdateDestination : DirectoryUpdateDestination
    {
        public AssemblyUpdateDestination(Assembly assembly)
            :base(GetDirectoryFromAssembly(assembly))
        {
        }

        private static string GetDirectoryFromAssembly(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            if (string.IsNullOrWhiteSpace(assembly.Location)) throw new ArgumentNullException(nameof(assembly), $"Location is null on assembly {assembly.FullName}");

            if (Directory.Exists(assembly.Location))
                return assembly.Location;
            else
                return Path.GetDirectoryName(assembly.Location);
        }
    }
}
