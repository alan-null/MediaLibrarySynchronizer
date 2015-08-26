using System.Linq;
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

        public static SyncVersion SetFieldValue(this SyncVersion syncVersion, IFieldInfo fieldInfo, string value)
        {
            SyncField field = syncVersion.Fields.FirstOrDefault(f => f.FieldID == fieldInfo.FieldId);
            var fieldIndex = syncVersion.Fields.IndexOf(field);
            syncVersion.Fields[fieldIndex].FieldValue = value;
            return syncVersion;
        }
    }
}
