using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ArchPM.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyManager
    {
        /// <summary>
        /// Loads the assemblies.
        /// </summary>
        /// <param name="directoryFolderPath">The directory folder path.</param>
        /// <returns></returns>
        public static IEnumerable<Assembly> LoadAssemblies(String directoryFolderPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryFolderPath);
            FileInfo[] files = directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

            foreach (FileInfo file in files)
            {
                // Load the file into the application domain.
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(file.FullName);
                Assembly assembly = AppDomain.CurrentDomain.Load(assemblyName);
                yield return assembly;
            }

            yield break;
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentAssembly">The current assembly.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetProvider<T>(Assembly currentAssembly)
        {
            List<T> result = new List<T>();

            var types = currentAssembly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsClass || type.IsNotPublic) continue;
                if (type.GetInterfaces().Contains(typeof(T)))
                {
                    try
                    {
                        var provider = (T)Activator.CreateInstance(type);
                        result.Add(provider);
                    }
                    catch { }

                }
            }

            return result;
        }

    }
}
