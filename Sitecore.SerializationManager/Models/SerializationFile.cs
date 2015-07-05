namespace Sitecore.SerializationManager.Models
{
    public class SerializationFile
    {
        public string Name { get; set; }
        public string Extensions { get; set; }
        public byte[] Blob { get; set; }

        public SerializationFile(string name, string extension, byte[] blob)
        {
            Name = name;
            Extensions = extension;
            Blob = blob;
        }
    }
}
