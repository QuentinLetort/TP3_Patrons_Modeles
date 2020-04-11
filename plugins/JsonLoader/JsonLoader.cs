using System.IO;
using System;
using System.Collections.Generic;
using PluginBase;
using Newtonsoft.Json;

namespace JsonLoader
{
    public class JsonLoader<T> : DataLoaderPlugin<T>
    {
        public string Name => "json";

        public string Description => "Load objects from JSON files.";

        private string[] listFiles(string path)
        {
            return Directory.GetFiles(path, "*.json");
        }
        public IEnumerable<T> LoadData(string path)
        {
            List<T> result = new List<T>();
            foreach (string file in listFiles(path))
            {
                Console.WriteLine("Loading file: " + file);
                result.AddRange(JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(file)));
            }
            return result;
        }
    }
}
