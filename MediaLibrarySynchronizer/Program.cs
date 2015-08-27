using System;
using System.IO;
using MediaLibrarySynchronizer.ConfigurationProviders.FileToSyncItem;
using MediaLibrarySynchronizer.Converter.Converters;

namespace MediaLibrarySynchronizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string configPath = GetConfigPath();
            var configProvider = new JsonConfigurationProvider<FileToSyncItemConfiguration>(configPath);
            var configurationWrapper = configProvider.GetConfig();

            if (configurationWrapper != null)
            {
                var serializationConfig = configurationWrapper.SerializationManagerConfig;
                foreach (FileToSyncItemConfiguration configuration in configurationWrapper.SynchronizationConfigs)
                {
                    var fileToSyncItemConverter = new FileToSyncItemConverter(configuration, serializationConfig);
                    fileToSyncItemConverter.Convert();
                }
            }
        }

        private static string GetConfigPath()
        {
            return String.Format("{0}\\config.json", Directory.GetCurrentDirectory());
        }
    }
}
