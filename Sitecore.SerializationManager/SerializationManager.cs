using System;
using System.IO;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.SerializationManager.Constants;
using Sitecore.SerializationManager.Extensions;
using Sitecore.SerializationManager.Models;

namespace Sitecore.SerializationManager
{
    public class SerializationManager
    {
        public void AttachFileToSerializationItem(string itemPath, string filePath)
        {
            SyncItem syncItem = SyncItemProvider.GetSyncItem(itemPath);

            byte[] bytes = ReadFile(filePath);
            var blobValue = System.Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);

            syncItem.SetFieldValue(FileTemplateFields.Blob, blobValue);
            syncItem.SetFieldValue(FileTemplateFields.Size, bytes.Length.ToString());

            SyncItemProvider.SaveSyncItem(syncItem, itemPath);
        }

        public void DetachFileFromSerializationItem(string itemPath)
        {
            SyncItem syncItem = SyncItemProvider.GetSyncItem(itemPath);

            syncItem.RemoveField(FileTemplateFields.Blob.FieldId);
            syncItem.SetFieldValue(FileTemplateFields.Size, String.Empty);
            syncItem.SetFieldValue(FileTemplateFields.Extension, String.Empty);
            syncItem.SetFieldValue(FileTemplateFields.MimeType, String.Empty);

            SyncItemProvider.SaveSyncItem(syncItem, itemPath);
        }

        public SerializationFile DownloadFileFromSerializationItem(string itemPath)
        {
            SyncItem syncItem = SyncItemProvider.GetSyncItem(itemPath);

            string blobValue = syncItem.SharedValues[FileTemplateFields.Blob.FieldId];
            string extension = syncItem.SharedValues[FileTemplateFields.Extension.FieldId];
            byte[] fromBase64String = System.Convert.FromBase64String(blobValue);

            return new SerializationFile(syncItem.Name, extension, fromBase64String);
        }

        private static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
    }
}
