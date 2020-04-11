using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginBase;

namespace AppWithPlugin
{
    class Program
    {
        static string GetRootDirectory()
        {
            return Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(
                            Path.GetDirectoryName(
                                Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));
        }
        static Assembly LoadPlugin(string relativePath)
        {
            // Navigate up to the solution root
            string root = GetRootDirectory();
            string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
            Console.WriteLine($"Loading loaders from: {pluginLocation}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }



        static IEnumerable<DataLoaderPlugin<T>> CreateDataLoaders<T>(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(DataLoaderPlugin<>).IsAssignableFromGeneric(type))
                {
                    Type[] typeArgs = { typeof(T) };
                    Type constructed = type.MakeGenericType(typeArgs);
                    DataLoaderPlugin<T> result = Activator.CreateInstance(constructed) as DataLoaderPlugin<T>;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
                throw new ApplicationException(
                    $"Can't find any type which implements DataLoaderPlugin<T> in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }

        static void showSample<T>(List<T> list, int size)
        {
            Console.WriteLine($"Sample of {size}:");
            foreach (T el in list)
            {
                if (size == 0)
                {
                    break;
                }
                Console.WriteLine(el);
                size--;
            }
        }
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1 && args[0] == "/d")
                {
                    Console.WriteLine("Waiting for any key...");
                    Console.ReadLine();
                }

                string[] pluginPaths = new string[]
                {
                   @"plugins\JsonLoader\bin\Debug\netcoreapp3.1\JsonLoader.dll",
                   @"plugins\CsvLoader\bin\Debug\netcoreapp3.1\CsvLoader.dll",
                };

                IEnumerable<DataLoaderPlugin<User>> userLoaders = pluginPaths.SelectMany(pluginPath =>
                {
                    Assembly pluginAssembly = LoadPlugin(pluginPath);
                    return CreateDataLoaders<User>(pluginAssembly);
                }).ToList();
                try
                {
                    string selectedLoader = args[0];
                    Console.WriteLine($"-- {selectedLoader} --");
                    DataLoaderPlugin<User> loader = userLoaders.FirstOrDefault(l => l.Name == selectedLoader);
                    if (loader == null)
                    {
                        Console.WriteLine("No such loader is known.");
                        return;
                    }
                    string dataDir = Path.GetFullPath(Path.Combine(GetRootDirectory(), args[1].Replace('\\', Path.DirectorySeparatorChar)));
                    List<User> users = loader.LoadData(dataDir).ToList();
                    showSample<User>(users, 10);
                    Console.WriteLine("Total number of users: " + users.Count);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Loaders: ");
                    foreach (DataLoaderPlugin<User> loader in userLoaders)
                    {
                        Console.WriteLine($"{loader.Name.ToUpper()}\t - {loader.Description}\t - Usage: {loader.Name} [DATADIR]");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}