using System;
using System.Linq;

namespace Sitecore.SerializationManager.Utils
{
    public class SitecoreUtils
    {
        public static string GenerateIconValue(string id)
        {
            return String.Format("~/media/{0}.ashx?h=16&thn=1&w=16", MainUtil.ShortenGuid(id));
        }

        public static string GetTemplateName(string templatePath)
        {
            return templatePath.Split('/').Last();
        }
    }
}
