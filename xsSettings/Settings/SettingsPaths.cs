using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsPaths
    {
        public class SettingsPathData
        {
            public SettingsPathData()
            {
                /* Default constructor */
            }

            public SettingsPathData(string id)
            {
                Id = id;
            }

            [XmlAttribute("id")]
            public string Id { get; set; }

            [XmlAttribute("location")]
            public string Location { get; set; }

            public override string ToString()
            {
                return Location;
            }
        }

        [XmlElement("path")]
        public List<SettingsPathData> Path = new List<SettingsPathData>();

        public SettingsPathData GetPath(string id)
        {
            if (string.IsNullOrEmpty(id)) { return null; }
            var settingsPathData = Path.FirstOrDefault(o => o.Id.ToLower() == id.ToLower());
            if (settingsPathData == null)
            {
                /* Doesn't exists? add a new one */
                settingsPathData = new SettingsPathData(id);
                Path.Add(settingsPathData);
            }
            return settingsPathData;
        }
    }
}