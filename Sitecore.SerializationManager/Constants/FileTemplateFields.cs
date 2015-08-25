using Sitecore.SerializationManager.Models;

namespace Sitecore.SerializationManager.Constants
{
    public class FileTemplateFields : IFieldInfo
    {
        private readonly string _name;
        private readonly string _id;

        public static readonly FileTemplateFields Blob = new FileTemplateFields("Blob", "{40E50ED9-BA07-4702-992E-A912738D32DC}");
        public static readonly FileTemplateFields FilePath = new FileTemplateFields("FilePath", "{2134867A-AC67-4DAC-836C-A9264FD9D6D6}");
        public static readonly FileTemplateFields Title = new FileTemplateFields("Title", "{3F4B20E9-36E6-4D45-A423-C86567373F82}");
        public static readonly FileTemplateFields Keywords = new FileTemplateFields("Keywords", "{2FAFE7CB-2691-4800-8848-255EFA1D31AA}");
        public static readonly FileTemplateFields Description = new FileTemplateFields("Description", "{BA8341A1-FF30-47B8-AE6A-F4947E4113F0}");
        public static readonly FileTemplateFields Extension = new FileTemplateFields("Extension", "{C06867FE-9A43-4C7D-B739-48780492D06F}");
        public static readonly FileTemplateFields MimeType = new FileTemplateFields("MimeType", "{6F47A0A5-9C94-4B48-ABEB-42D38DEF6054}");
        public static readonly FileTemplateFields Size = new FileTemplateFields("Size", "{6954B7C7-2487-423F-8600-436CB3B6DC0E}");
        public static readonly FileTemplateFields Format = new FileTemplateFields("Format", "{FBD90442-DB0C-48F1-A0F4-0427CD9AFCCE}");
        public static readonly FileTemplateFields CountryCode = new FileTemplateFields("CountryCode", "{FF01BC1A-EF22-407C-9115-7DBEE11451A5}");
        public static readonly FileTemplateFields LocationDescription = new FileTemplateFields("LocationDescription", "{E8FA62D2-13FC-4C5C-91E5-71D615591420}");
        public static readonly FileTemplateFields Latitude = new FileTemplateFields("Latitude", "{5D589EC4-4842-4630-A860-540357A196F3}");
        public static readonly FileTemplateFields Longitude = new FileTemplateFields("Longitude", "{11CB6358-8F71-4F80-AB30-04D745D99D56}");
        public static readonly FileTemplateFields ZipCode = new FileTemplateFields("ZipCode", "{FB540BF1-B2EB-47C9-A55F-F01B2FCA5851}");
        public static readonly FileTemplateFields Icon = new FileTemplateFields("Icon", "{06D5295C-ED2F-4A54-9BF2-26228D113318}");

        public string Name
        {
            get { return _name; }
        }

        public string Key
        {
            get { return _name.ToLower(); }
        }

        public string FieldId
        {
            get { return _id; }
        }

        private FileTemplateFields(string name, string id)
        {
            _name = name;
            _id = id;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
