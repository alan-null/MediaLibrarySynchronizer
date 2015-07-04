using System.IO;
using Sitecore.Data.Serialization.ObjectModel;

namespace Sitecore.SerializationManager
{
    public class SyncItemProvider
    {
        public static SyncItem GetSyncItem(string path)
        {
            using (TextReader reader = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                return SyncItem.ReadItem(new Tokenizer(reader));
            }
        }

        public static void SaveSyncItem(SyncItem item, string path)
        {
            using (var fileStream = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    item.Serialize(writer);
                }
            }
        }
    }
}
