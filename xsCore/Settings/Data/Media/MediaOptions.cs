/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace xsCore.Settings.Data.Media
{
    [Serializable]
    public class MediaOptions
    {
        public MediaOptions()
        {
            /* Default constructor */
        }

        public MediaOptions(List<MediaOptionData> options) : this(options, null)
        {
            /* Single paramater constructor */
        }

        public MediaOptions(List<MediaOptionData> options, IEnumerable<string> secondaryOptions)
        {
            if (secondaryOptions != null)
            {
                foreach (var sp in secondaryOptions.Select(s => s.Split('=')))
                {
                    Option.Add(new MediaOptionData(sp[0], sp.Length > 1 ? sp[1] : string.Empty));
                }
            }
            Option.AddRange(options.ToArray());
        }

        [XmlElement("option")]
        public List<MediaOptionData> Option = new List<MediaOptionData>();

        public MediaOptionData GetOption(string id)
        {
            if (string.IsNullOrEmpty(id)) { return null; }
            var playerOption = Option.FirstOrDefault(o => o.Id.ToLower() == id.ToLower());
            if (playerOption != null)
            {
                return playerOption;
            }
            playerOption = new MediaOptionData(id);
            Option.Add(playerOption);
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