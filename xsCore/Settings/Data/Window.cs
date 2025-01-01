/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Xml.Serialization;
using xsCore.Utils;

namespace xsCore.Settings.Data
{
    [Serializable]
    public class Window
    {
        public class WindowData
        {
            public WindowData()
            {
                /* Default constructor */
            }

            public WindowData(WindowData data)
            {
                Size = data.Size;
                Location = data.Location;
            }

            [XmlAttribute("location")]
            public string LocationString
            {
                get { return XmlFormatting.WritePointFormat(Location); }
                set { Location = XmlFormatting.ParsePointFormat(value); }
            }

            [XmlAttribute("size")]
            public string SizeString
            {
                get { return XmlFormatting.WriteSizeFormat(Size); }
                set { Size = XmlFormatting.ParseSizeFormat(value); }
            }

            [XmlIgnore]
            public Point Location { get; set; }

            [XmlIgnore]
            public Size Size { get; set; }
        }

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