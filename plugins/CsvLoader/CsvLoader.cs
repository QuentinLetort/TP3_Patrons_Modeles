using System.IO;
using System;
using PluginBase;
using System.Collections.Generic;
using CsvHelper;
using System.Globalization;

namespace CsvLoader
{
    public class CsvLoader<T> : DataLoaderPlugin<T>
    {
        public string Name => "csv";

        public string Description => "Load objects from CSV files.";

        private string[] listFiles(string path)
        {
            return Directory.GetFiles(path, "*.csv");
        }
        public IEnumerable<T> LoadData(string path)
        {
            List<T> result = new List<T>();
            foreach (string file in listFiles(path))
            {
                Console.WriteLine("Loading file: " + file);
                using (StreamReader reader = new StreamReader(file))
                using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    result.AddRange(csv.GetRecords<T>());
                }
            }
            return result;
        }
    }
}
