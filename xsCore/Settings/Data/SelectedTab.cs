/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;

namespace xsCore.Settings.Data
{
    [Serializable]
    public class SelectedTab
    {
        public SelectedTab()
        {
            /* Default constructor */
        }

        public SelectedTab(SelectedTab tab)
        {
            /* Copy construstor */
            Id = tab.Id;
        }

        [XmlAttribute("id")]
        public string Id { get; set; }
    }
}
