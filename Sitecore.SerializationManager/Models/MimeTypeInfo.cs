namespace Sitecore.SerializationManager.Models
{
    public class MimeTypeInfo
    {
        public string MimeType { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }

        public MimeTypeInfo(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }
    }
}