using System;
using System.Xml.Serialization;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsSelectedTab
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }
}
