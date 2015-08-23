namespace Sitecore.SerializationManager.Models
{
    public class MediaTypeConfig
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public string SharedTemplate { get; set; }
        public string VersionedTemplate { get; set; }

        public MediaTypeConfig(string name, string extension)
        {
            Name = name;
            Extension = extension;
        }
    }
}