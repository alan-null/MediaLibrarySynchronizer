using System;
using System.Collections.Generic;
using System.Linq;
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
            string key = _mappings.Keys.FirstOrDefault(s => path.ToLower().Contains(s.ToLower()));
            return key != null ? _mappings[key] : String.Empty;
        }
    }
}
