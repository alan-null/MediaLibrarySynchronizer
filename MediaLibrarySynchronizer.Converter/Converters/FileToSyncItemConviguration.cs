using MediaLibrarySynchronizer.Converter.Models;

namespace MediaLibrarySynchronizer.Converter.Converters
{
    public class FileToSyncItemConviguration
    {
        public string SourcePath { get; set; }
        public Paths Destination { get; set; }

        public FileToSyncItemConviguration() { }

        public FileToSyncItemConviguration(string sourcePath, Paths destination)
        {
            Destination = destination;
            SourcePath = sourcePath;
        }

        public FileToSyncItemConviguration(FileToSyncItemConviguration source)
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