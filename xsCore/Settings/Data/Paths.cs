/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace xsCore.Settings.Data
{
    [Serializable]
    public class Paths
    {
        public class PathData
        {
            public PathData()
            {
                /* Default constructor */
            }

            //public SettingsPathData(SettingsPathData data)
            //{
            //    Id = data.Id;
            //    Location = data.Location;
            //}

            public PathData(string id)
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

        public Paths() 
        {
            /* Default constructor */
        }

        public Paths(Paths paths)
        {
            /* Copy constructor */
            Path = new List<PathData>(paths.Path);
        }

        [XmlElement("path")]
        public List<PathData> Path = new List<PathData>();

        public PathData GetPath(string id)
        {
            if (string.IsNullOrEmpty(id)) { return null; }
            var settingsPathData = Path.FirstOrDefault(o => string.Equals(o.Id, id, StringComparison.CurrentCultureIgnoreCase));
            if (settingsPathData == null)
            {
                /* Doesn't exists? add a new one */
                settingsPathData = new PathData(id);
                Path.Add(settingsPathData);
            }
            return settingsPathData;
        }
    }
}