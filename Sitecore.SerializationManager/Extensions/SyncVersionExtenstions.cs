using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.SerializationManager.Models;

namespace Sitecore.SerializationManager.Extensions
{
    static class SyncVersionExtenstions
    {
        public static SyncVersion AddField(this SyncVersion syncItem, IFieldInfo field, string value)
        {
            syncItem.AddField(field.FieldId, field.Name, field.Key, value, true);
            return syncItem;
        }
    }
}
