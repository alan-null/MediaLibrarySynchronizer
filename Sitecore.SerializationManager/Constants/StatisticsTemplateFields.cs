using Sitecore.SerializationManager.Models;

namespace Sitecore.SerializationManager.Constants
{
    public sealed class StatisticsTemplateFields : IFieldInfo
    {
        private readonly string _name;
        private readonly string _id;

        public static readonly StatisticsTemplateFields Created = new StatisticsTemplateFields("__Created", FieldIDs.Created.ToString());
        public static readonly StatisticsTemplateFields CreatedBy = new StatisticsTemplateFields("__Created by", FieldIDs.CreatedBy.ToString());
        public static readonly StatisticsTemplateFields Revision = new StatisticsTemplateFields("__Revision", FieldIDs.Revision.ToString());
        public static readonly StatisticsTemplateFields Updated = new StatisticsTemplateFields("__Updated", FieldIDs.Updated.ToString());
        public static readonly StatisticsTemplateFields UpdatedBy = new StatisticsTemplateFields("__Updated by", FieldIDs.UpdatedBy.ToString());

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

        private StatisticsTemplateFields(string name, string id)
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
