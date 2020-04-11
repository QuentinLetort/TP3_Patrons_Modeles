using System.Collections.Generic;

namespace PluginBase
{
    public interface DataLoaderPlugin<T>
    {
        string Name { get; }
        string Description { get; }

        IEnumerable<T> LoadData(string relativePath);
    }
}