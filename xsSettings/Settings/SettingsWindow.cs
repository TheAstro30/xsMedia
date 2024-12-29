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
    public class SettingsWindow
    {
        public SettingsWindow()
        {
            /* Default constructor */
        }

        public SettingsWindow(SettingsWindow window)
        {
            MainWindow = new SettingsWindowData(window.MainWindow);
            EffectsWindow = window.EffectsWindow;
            NetworkWindow = window.NetworkWindow;
            CurrentSkin = window.CurrentSkin;
        }

        [XmlAttribute("skin")]
        public string CurrentSkin { get; set; }

        [XmlElement("mainWindow")]
        public SettingsWindowData MainWindow = new SettingsWindowData();

        [XmlElement("effectsWindow")]
        public SettingsWindowData EffectsWindow = new SettingsWindowData();

        [XmlElement("networkWindow")]
        public SettingsWindowData NetworkWindow = new SettingsWindowData();
    }
}