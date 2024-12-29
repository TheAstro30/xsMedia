/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;

namespace xsSettings.Settings
{
    [Serializable]
    public class SettingsSelectedTab
    {
        public SettingsSelectedTab()
        {
            /* Default constructor */
        }

        public SettingsSelectedTab(SettingsSelectedTab tab)
        {
            /* Copy construstor */
            Id = tab.Id;
        }

        [XmlAttribute("id")]
        public string Id { get; set; }
    }
}
