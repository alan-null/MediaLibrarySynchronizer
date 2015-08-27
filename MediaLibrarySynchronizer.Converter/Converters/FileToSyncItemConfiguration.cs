using MediaLibrarySynchronizer.Converter.Models;

namespace MediaLibrarySynchronizer.Converter.Converters
{
    public class FileToSyncItemConfiguration
    {
        public string SourcePath { get; set; }
        public Paths Destination { get; set; }

        public FileToSyncItemConfiguration() { }

        public FileToSyncItemConfiguration(string sourcePath, Paths destination)
        {
            Destination = destination;
            SourcePath = sourcePath;
        }

        public FileToSyncItemConfiguration(FileToSyncItemConfiguration source)
        {
            SourcePath = source.SourcePath;
            Destination = new Paths
            {
                DataFolder = source.Destination.DataFolder,
                Database = source.Destination.Database,
                SitecorePath = source.Destination.SitecorePath
            };
        }
    }
}