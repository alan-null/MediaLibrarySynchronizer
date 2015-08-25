using System;
using System.IO;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.SerializationManager.Constants;
using Sitecore.SerializationManager.Extensions;
using Sitecore.SerializationManager.Models;
using Sitecore.SerializationManager.Utils;

namespace Sitecore.SerializationManager
{
    public class SerializationManager
    {
        private readonly SerializationManagerConfig _config;

        public SerializationManager(SerializationManagerConfig config)
        {
            _config = config;
        }

        public void AttachFileToSerializationItem(string itemPath, string filePath)
        {
            SyncItem syncItem = SyncItemProvider.GetSyncItem(itemPath);
            syncItem.AttachMediaFile(new FileInfo(filePath));
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

        public SyncItem CreateSyncMediaItem(string itemPath, string parentID, string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            var templatePath = MediaTypeResolver.Instance.GetTemplate(fileInfo.Extension.TrimStart('.'), false);
            string name = Path.GetFileNameWithoutExtension(fileInfo.Name);
            SyncItem syncItem = CreateSyncItem(name, itemPath, parentID, templatePath);
            syncItem.AttachMediaFile(fileInfo);
            return syncItem;
        }

        public SyncItem CreateSyncMediaFolder(string name, string itemPath, string parentID)
        {
            SyncItem syncItem = CreateSyncItem(name, itemPath, parentID, Paths.MediaFolderPath);
            return syncItem;
        }

        public SyncItem CreateSyncItem(string name, string itemPath, string parentID, string templatePath)
        {
            SyncItem syncItem = new SyncItem
            {
                ID = MainUtil.GetNewID(),
                DatabaseName = _config.CurrentDatabase,
                ItemPath = String.Format("{0}/{1}", itemPath, name),
                ParentID = parentID,
                Name = name,
                MasterID = Guid.Empty.ToString(),
                TemplateID = TemplateIdResolver.Instance.GetTemplateId(templatePath),
                TemplateName = SitecoreUtils.GetTemplateName(templatePath)
            };
            syncItem.AddVersion(_config.BuildSyncVersion());
            return syncItem;
        }
    }
}
