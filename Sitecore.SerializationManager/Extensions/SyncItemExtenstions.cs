using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Serialization.ObjectModel;

namespace Sitecore.SerializationManager.Extensions
{
    static class SyncItemExtenstions
    {
        public static SyncItem ChangeFieldValue(this SyncItem syncItem, string fieldId, string value)
        {
            int fieldIndex = syncItem.SharedFields.ToList().FindIndex(field => field.FieldID == fieldId);
            syncItem.SharedFields[fieldIndex].FieldValue = value;
            return syncItem;
        }

        public static SyncItem AddSharedField(this SyncItem syncItem, SyncField syncField)
        {
            List<SyncField> syncFields = syncItem.SharedFields.ToList();
            foreach (SyncField f in syncFields)
            {
                syncItem.AddSharedField(f.FieldID, f.FieldName, f.FieldKey, f.FieldValue, f.FieldValue != null);
            }
            return syncItem;
        }
    }
}
