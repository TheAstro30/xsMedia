/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
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

            //public SettingsPathData(SettingsPathData data)
            //{
            //    Id = data.Id;
            //    Location = data.Location;
            //}

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

        public SettingsPaths() 
        {
            /* Default constructor */
        }

        public SettingsPaths(SettingsPaths paths)
        {
            /* Copy constructor */
            Path = new List<SettingsPathData>(paths.Path);
        }

        [XmlElement("path")]
        public List<SettingsPathData> Path = new List<SettingsPathData>();

        public SettingsPathData GetPath(string id)
        {
            if (string.IsNullOrEmpty(id)) { return null; }
            var settingsPathData = Path.FirstOrDefault(o => string.Equals(o.Id, id, StringComparison.CurrentCultureIgnoreCase));
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