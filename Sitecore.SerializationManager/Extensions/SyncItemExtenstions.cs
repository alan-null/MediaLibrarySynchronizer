using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.SerializationManager.Constants;
using Sitecore.SerializationManager.Utils;

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

        public static SyncItem AddVersion(this SyncItem syncItem, SyncVersion syncVersion)
        {
            syncItem.Versions.Add(syncVersion);
            return syncItem;
        }

        public static SyncItem AttachMediaFile(this SyncItem syncItem, FileInfo fileInfo)
        {
            byte[] bytes = FileUtils.ReadFile(fileInfo.FullName);
            var blobValue = System.Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
            string extension = fileInfo.Extension.TrimStart('.');
            string mimeType = MediaTypeResolver.Instance.ResolveMimeType(extension);
            syncItem.SetFieldValue(FileTemplateFields.Blob, blobValue);
            syncItem.SetFieldValue(FileTemplateFields.Size, bytes.Length.ToString());
            syncItem.SetFieldValue(FileTemplateFields.Extension, extension);
            syncItem.SetFieldValue(FileTemplateFields.MimeType, mimeType);
            syncItem.SetFieldValue(FileTemplateFields.Icon, SitecoreUtils.GenerateIconValue(syncItem.ID));
            return syncItem;
        }

    }
}
