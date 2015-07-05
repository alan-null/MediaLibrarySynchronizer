using System;
using System.IO;
using Sitecore.Data.Serialization.ObjectModel;
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

            syncItem.ChangeFieldValue(Constans.FieldIDs.Blob, blobValue);
            syncItem.ChangeFieldValue(Constans.FieldIDs.Size, bytes.Length.ToString());

            SyncItemProvider.SaveSyncItem(syncItem, itemPath);
        }

        public void DetachFileFromSerializationItem(string itemPath)
        {
            SyncItem syncItem = SyncItemProvider.GetSyncItem(itemPath);

            syncItem.RemoveField(Constans.FieldIDs.Blob);
            syncItem.ChangeFieldValue(Constans.FieldIDs.Size, String.Empty);
            syncItem.ChangeFieldValue(Constans.FieldIDs.Extension, String.Empty);
            syncItem.ChangeFieldValue(Constans.FieldIDs.MimeType, String.Empty);

            SyncItemProvider.SaveSyncItem(syncItem, itemPath);
        }

        public SerializationFile DownloadFileFromSerializationItem(string itemPath)
        {
            SyncItem syncItem = SyncItemProvider.GetSyncItem(itemPath);

            string blobValue = syncItem.SharedValues[Constans.FieldIDs.Blob];
            string extension = syncItem.SharedValues[Constans.FieldIDs.Extension];
            byte[] fromBase64String = System.Convert.FromBase64String(blobValue);

            return new SerializationFile(syncItem.Name, extension, fromBase64String);
        }

        private byte[] ReadFile(string filePath)
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
