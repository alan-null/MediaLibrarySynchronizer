using Sitecore.Data.Serialization.ObjectModel;

namespace Sitecore.SerializationManager.Models
{
    public class ItemVersion
    {
        public string Language { get; set; }
        public string Version { get; set; }

        public bool Equals(SyncVersion obj)
        {
            return Language == obj.Language && Version == obj.Version;
        }
    }
}
