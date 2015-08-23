using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Win32;
using Sitecore.Collections;
using Sitecore.Diagnostics;
using Sitecore.SerializationManager.Models;
using Sitecore.SerializationManager.Resources;
using Sitecore.Xml;

namespace Sitecore.SerializationManager
{
    public sealed class MimeTypeResolver
    {
        private static MimeTypeResolver _instance;
        public static MimeTypeResolver Instance
        {
            get { return _instance ?? (_instance = new MimeTypeResolver()); }
        }
        private readonly SafeDictionary<string, MediaTypeConfig> _mediaTypes = new SafeDictionary<string, MediaTypeConfig>(StringComparer.OrdinalIgnoreCase);

        private MimeTypeResolver()
        {
            XmlDocument doc = new XmlDocument();
                doc.LoadXml(MimeTypes.Configuration);
            XmlNode newNode = doc.DocumentElement;
            ParseMediaTypes(newNode);
        }

        public string ResolveMimeType(string extension)
        {
            string mimeType = (GetMimeTypeFromConfig(extension) ?? GetMimeTypeFromRegistry(extension)) ?? GetMimeTypeFromWellknownTypes(extension);
            return mimeType ?? string.Empty;
        }

        public string GetTemplate(string extension, bool versioned)
        {
            Assert.ArgumentNotNull((object)extension, "extension");
            MediaTypeConfig mediaTypeConfig = GetMediaTypeInfoConfig(extension);
            if (mediaTypeConfig == null)
                return null;
            if (!versioned)
                return mediaTypeConfig.SharedTemplate;
            return mediaTypeConfig.VersionedTemplate;
        }


        private string GetMimeTypeFromConfig(string extension)
        {
            MediaTypeConfig mediaTypeConfig = GetMediaTypeInfoConfig(extension);
            if (mediaTypeConfig == null)
                return null;
            return mediaTypeConfig.MimeType;
        }

        private MediaTypeConfig GetMediaTypeInfoConfig(string extension)
        {
            Assert.ArgumentNotNull(extension, "extension");
            lock (_mediaTypes.SyncRoot)
            {
                return _mediaTypes[extension] ?? _mediaTypes["*"];
            }
        }

        private string GetMimeTypeFromRegistry(string extension)
        {
            try
            {
                RegistryKey registryKey = Registry.ClassesRoot;
                foreach (string str in registryKey.OpenSubKey("MIME\\Database\\Content Type").GetSubKeyNames())
                {
                    string registry = StringUtil.GetString(registryKey.OpenSubKey("MIME\\Database\\Content Type\\" + str).GetValue("Extension")).TrimStart('.');
                    if (registry.Equals(extension, StringComparison.OrdinalIgnoreCase))
                    {
                        return str;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error trying to determine MIME type for '" + extension + "'", ex, typeof(MimeTypeResolver));
            }
            return null;
        }

        private string GetMimeTypeFromWellknownTypes(string extension)
        {
            switch (extension)
            {
                case "bmp":
                    return "image/bmp";
                case "emf":
                    return "image/emf";
                case "gif":
                    return "image/gif";
                case "icon":
                    return "image/icon";
                case "jpg":
                case "jpeg":
                    return "image/jpeg";
                case "pdf":
                    return "application/pdf";
                case "png":
                    return "image/png";
                case "tiff":
                    return "image/tiff";
                case "wmf":
                    return "image/wmf";
                case "zip":
                    return "application/x-zip-compressed";
                default:
                    return null;
            }
        }

        private void ParseMediaTypes(XmlNode childNode)
        {
            if (childNode != null)
            {
                List<XmlNode> childNodes = XmlUtil.GetChildNodes("mediaType", childNode);
                if (childNodes != null)
                {
                    XmlNode defaultNode = childNodes.FirstOrDefault(n => XmlUtil.GetAttribute("extensions", n).Contains("*"));
                    foreach (XmlNode configNode1 in childNodes)
                    {
                        ParseMediaType(configNode1, defaultNode);
                    }
                }
            }
        }

        private void ParseMediaType(XmlNode configNode, XmlNode defaultNode)
        {
            Assert.ArgumentNotNull(configNode, "configNode");
            string attribute = XmlUtil.GetAttribute("extensions", configNode);
            if (attribute.Length != 0)
            {
                string[] extensions = StringUtil.Split(attribute, ',', true);
                lock (_mediaTypes.SyncRoot)
                {
                    foreach (string ext in extensions)
                    {
                        if (_mediaTypes.Keys.Contains(ext))
                        {
                            FillConfiguration(_mediaTypes[ext], configNode, defaultNode);
                        }
                        else
                        {
                            MediaTypeConfig config = new MediaTypeConfig(XmlUtil.GetAttribute("name", configNode, attribute), attribute);
                            FillConfiguration(config, configNode, defaultNode);
                            _mediaTypes[ext] = config;
                        }
                    }
                }
            }
        }

        private void FillConfiguration(MediaTypeConfig config, XmlNode configNode, XmlNode defaultNode)
        {
            config.MimeType = GetChildValue("mimeType", configNode, defaultNode, config.MimeType);
            config.SharedTemplate = GetChildValue("sharedTemplate", configNode, defaultNode, config.SharedTemplate);
            config.VersionedTemplate = GetChildValue("versionedTemplate", configNode, defaultNode, config.VersionedTemplate);
        }

        private string GetChildValue(string nodeName, XmlNode node, XmlNode defaultNode, string oldValue)
        {
            Assert.ArgumentNotNull(nodeName, "nodeName");
            Assert.ArgumentNotNull(node, "node");
            XmlNode childNode = XmlUtil.GetChildNode(nodeName, node);
            if (childNode == null)
            {
                if (string.IsNullOrEmpty(oldValue))
                {
                    return defaultNode != null ? XmlUtil.GetChildValue(nodeName, defaultNode) : string.Empty;
                }
                return oldValue;
            }
            return XmlUtil.GetValue(childNode);
        }
    }
}
