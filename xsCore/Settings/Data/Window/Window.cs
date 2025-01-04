/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Xml.Serialization;

namespace xsCore.Settings.Data.Window
{
    [Serializable]
    public class Window
    {
        public Window()
        {
            /* Default constructor */
        }

        public Window(Window window)
        {
            MainWindow = new WindowData(window.MainWindow);
            EffectsWindow = window.EffectsWindow;
            NetworkWindow = window.NetworkWindow;
            FavoritesWindow = window.FavoritesWindow;
            CurrentSkin = window.CurrentSkin;
        }

        [XmlAttribute("skin")]
        public string CurrentSkin { get; set; }

        [XmlElement("mainWindow")]
        public WindowData MainWindow = new WindowData();

        [XmlElement("effectsWindow")]
        public WindowData EffectsWindow = new WindowData();

        [XmlElement("networkWindow")]
        public WindowData NetworkWindow = new WindowData();

        [XmlElement("favoritesWindow")]
        public WindowData FavoritesWindow = new WindowData();
    }
}