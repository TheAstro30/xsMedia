/* xsMedia - xsSettings
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using xsCore.Utils.SystemUtils;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsMediaOptions
    {
        public class MediaOption
        {
            /* Constructors */
            public MediaOption()
            {
                /* Empty constructor */
            }

            public MediaOption(string id)
            {
                Id = id;
            }

            public MediaOption(string id, string data)
            {
                Id = id;
                Data = data;
            }

            [XmlAttribute("id")]
            public string Id { get; set; }

            [XmlAttribute("data")]
            public string Data { get; set; }

            /* Formatting method for libVlc --option=data */
            public override string ToString()
            {
                return string.IsNullOrEmpty(Data) ? Id : string.Format("{0}={1}", Id, AppPath.MainDir(Data));
            }
        }

        public SettingsMediaOptions()
        {
            /* Default constructor */
        }

        public SettingsMediaOptions(List<MediaOption> options) : this(options, null)
        {
            /* Single paramater constructor */
        }

        public SettingsMediaOptions(List<MediaOption> options, IEnumerable<string> secondaryOptions)
        {
            if (secondaryOptions != null)
            {
                foreach (var sp in secondaryOptions.Select(s => s.Split('=')))
                {
                    Option.Add(new MediaOption(sp[0], sp.Length > 1 ? sp[1] : string.Empty));
                }
            }
            Option.AddRange(options.ToArray());
        }

        [XmlElement("option")]
        public List<MediaOption> Option = new List<MediaOption>();

        public MediaOption GetOption(string id)
        {
            if (string.IsNullOrEmpty(id)) { return null; }
            var playerOption = Option.FirstOrDefault(o => o.Id.ToLower() == id.ToLower());
            if (playerOption == null)
            {
                playerOption = new MediaOption(id);
                Option.Add(playerOption);
            }
            return playerOption;
        }

        public void RemoveOption(string id)
        {
            foreach (var playerOption in Option.Where(o => o.Id.ToLower() == id.ToLower()))
            {
                Option.Remove(playerOption);
                break;
            }
        }

        public string[] ToArray()
        {
            return Option.Select(s => s.ToString()).ToArray();
        }
    }
}