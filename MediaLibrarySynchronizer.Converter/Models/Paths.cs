using System;
using System.IO;

namespace MediaLibrarySynchronizer.Converter.Models
{
    public class Paths
    {
        public string DataFolder { get; set; }

        public string Database { get; set; }

        public string SitecorePath { get; set; }

        public string SerializationFolder
        {
            get { return Path.Combine(DataFolder, "serialization"); }
        }
        
        public string FullPath
        {
            get { return Path.Combine(SerializationFolder, Database, SitecorePath); }
        }

        public string Parent
        {
            get
            {
                string path = SitecorePath.TrimEnd('/');
                var i = path.LastIndexOf("/", StringComparison.Ordinal);
                return Path.Combine(SerializationFolder, Database, path.Substring(0, i));
            }
        }
    }
}