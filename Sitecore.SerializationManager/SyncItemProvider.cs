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
            TextWriter writer = (TextWriter)new StreamWriter((Stream)File.Create(path));
            item.Serialize(writer);
        }
    }
}
