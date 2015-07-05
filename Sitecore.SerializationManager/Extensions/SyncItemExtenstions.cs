using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.SerializationManager.Constants;

namespace Sitecore.SerializationManager.Extensions
{
    static class SyncItemExtenstions
    {
        public static SyncItem SetFieldValue(this SyncItem syncItem, FileTemplateFields fieldId, string value)
        {
            int fieldIndex = syncItem.SharedFields.ToList().FindIndex(field => field.FieldID == fieldId.FieldId);
            if (fieldIndex < 0)
            {
                SyncField filed = new SyncField
                {
                    FieldID = fieldId.FieldId,
                    FieldName = fieldId.Name,
                    FieldKey = fieldId.Key,
                    FieldValue = value
                };
                syncItem.SharedFields.Add(filed);
            }
            else
            {
                syncItem.SharedFields[fieldIndex].FieldValue = value;
            }

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
