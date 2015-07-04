using System;
using System.IO;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.SerializationManager.Extensions;

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
