using System.IO;
using Newtonsoft.Json;

namespace MediaLibrarySynchronizer.ConfigurationProviders.FileToSyncItem
{
    class JsonConfigurationProvider<T> : IConfigProvider<T>
    {
        private readonly ConfigurationWrapper<T> _configurationWrapper;

        public JsonConfigurationProvider(string path)
        {
            if (File.Exists(path))
            {
                string serializedConfiguration = File.ReadAllText(path);
                _configurationWrapper = JsonConvert.DeserializeObject<ConfigurationWrapper<T>>(serializedConfiguration);
            }
        }

        public ConfigurationWrapper<T> GetConfig()
        {
            return _configurationWrapper;
        }
    }
}
