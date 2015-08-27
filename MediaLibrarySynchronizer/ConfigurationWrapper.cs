using System.Collections.Generic;
using Sitecore.SerializationManager.Models;

namespace MediaLibrarySynchronizer
{
    public class ConfigurationWrapper<T>
    {
        public SerializationManagerConfig SerializationManagerConfig { get; set; }
        public IEnumerable<T> SynchronizationConfigs { get; set; }

        public ConfigurationWrapper() { }

        public ConfigurationWrapper(SerializationManagerConfig serializationManagerConfig, IEnumerable<T> synchronizationConfigs)
        {
            SerializationManagerConfig = serializationManagerConfig;
            SynchronizationConfigs = synchronizationConfigs;
        }
    }
}
