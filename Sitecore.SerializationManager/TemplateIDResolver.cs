using System.Collections.Generic;
using Newtonsoft.Json;
using Sitecore.SerializationManager.Resources;

namespace Sitecore.SerializationManager
{
    class TemplateIdResolver
    {
        private static TemplateIdResolver _instance;
        public static TemplateIdResolver Instance
        {
            get { return _instance ?? (_instance = new TemplateIdResolver()); }
        }
        private readonly Dictionary<string, string> _mappings;

        private TemplateIdResolver()
        {
            _mappings = JsonConvert.DeserializeObject<Dictionary<string, string>>(Templates.PathToIDMapping);
        }

        public string GetTemplateId(string path)
        {
            return _mappings[path];
        }
    }
}
