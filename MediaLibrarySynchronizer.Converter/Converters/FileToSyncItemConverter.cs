using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaLibrarySynchronizer.Converter.Models;
using Sitecore.Data.Serialization;
using Sitecore.Data.Serialization.ObjectModel;
using Sitecore.SerializationManager;
using Sitecore.SerializationManager.Constants;
using Sitecore.SerializationManager.Extensions;
using Sitecore.SerializationManager.Models;

namespace MediaLibrarySynchronizer.Converter.Converters
{
    public class FileToSyncItemConverter : IConvert
    {
        private readonly FileToSyncItemConfiguration _config;
        private readonly SerializationManager _serializationManager;

        public FileToSyncItemConverter(FileToSyncItemConfiguration config, SerializationManagerConfig serializationManager)
        {
            _config = config;
            _serializationManager = new SerializationManager(serializationManager);
        }

        public void Convert()
        {
            ConvertFolder(_config);
        }

        private void ConvertFolder(FileToSyncItemConfiguration path)
        {
            if (!Directory.Exists(path.Destination.FullPath))
            {
                Directory.CreateDirectory(path.Destination.FullPath);
            }
            if (!File.Exists(path.Destination.FullPath + PathUtils.Extension))
            {
                SyncItem parent = SyncItemProvider.GetSyncItem(path.Destination.Parent + PathUtils.Extension);
                var serializationFolder = _serializationManager.CreateSyncMediaFolder(new DirectoryInfo(path.Destination.FullPath).Name, path.Destination.Database, parent.ItemPath, parent.ID);
                SyncItemProvider.SaveSyncItem(serializationFolder, path.Destination.FullPath + PathUtils.Extension);
            }
            var mediaFiles = GetFiles(path.SourcePath).Where(NameIsNotNullOrEmpty);
            mediaFiles.ToList().ForEach(file => ConvertMediaFile(file, path));
            ConvertFolders(path);
        }

        private bool NameIsNotNullOrEmpty(MediaFile arg)
        {
            return !String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(arg.Name));
        }

        private void ConvertMediaFile(MediaFile mediaFile, FileToSyncItemConfiguration path)
        {
            if (File.Exists(path.Destination.FullPath + PathUtils.Extension))
            {
                SyncItem parent = SyncItemProvider.GetSyncItem(path.Destination.FullPath + PathUtils.Extension);
                string name = String.IsNullOrEmpty(Path.GetFileNameWithoutExtension(mediaFile.Name))
                    ? mediaFile.Name
                    : Path.GetFileNameWithoutExtension(mediaFile.Name);
                string syncItemPath = Path.Combine(path.Destination.FullPath, name + PathUtils.Extension);
                if (File.Exists(syncItemPath))
                {
                    SyncItem syncItem = SyncItemProvider.GetSyncItem(syncItemPath);
                    if (mediaFile.Blob != syncItem.SharedValues[FileTemplateFields.Blob.FieldId])
                    {
                        syncItem.AttachMediaFile(new FileInfo(mediaFile.FilePath));
                        syncItem = _serializationManager.UpdateStatistics(syncItem);
                        SyncItemProvider.SaveSyncItem(syncItem, syncItemPath);
                    }
                }
                else
                {
                    SyncItem item = _serializationManager.CreateSyncMediaItem(parent.ItemPath, path.Destination.Database, parent.ID, mediaFile.FilePath);
                    SyncItemProvider.SaveSyncItem(item, syncItemPath);
                }
            }
        }

        private void ConvertFolders(FileToSyncItemConfiguration path)
        {
            foreach (DirectoryInfo directoryInfo in GetFolders(path.SourcePath))
            {
                var newConfig = new FileToSyncItemConfiguration(path)
                {
                    SourcePath = directoryInfo.FullName,
                    Destination =
                    {
                        SitecorePath = path.Destination.SitecorePath + "/" + directoryInfo.Name
                    }
                };
                ConvertFolder(newConfig);
            }
        }

        private IEnumerable<DirectoryInfo> GetFolders(string path)
        {
            return Directory.GetDirectories(path).Select(s => new DirectoryInfo(s));
        }

        private IEnumerable<MediaFile> GetFiles(string sourcePath)
        {
            string[] strings = Directory.GetFiles(sourcePath);
            return strings.Select(s => new MediaFile(s));
        }
    }
}
