using System.IO;
using System.Windows.Forms;
using xsCore.Serialization;
using xsSettings.Forms;
using xsSettings.Internal;

namespace xsSettings
{
    public static class SettingsManager
    {
        static SettingsManager()
        {
            Settings = new PlayerSettings();
        }

        public static PlayerSettings Settings { get; set; }

        /* Load/Save functions */
        public static void Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                Settings = new PlayerSettings();
                XmlSerialize<PlayerSettings>.Save(fileName, Settings);
                return;
            }
            var tmp = new PlayerSettings();
            if (XmlSerialize<PlayerSettings>.Load(fileName, ref tmp))
            {
                Settings = tmp;
            }
        }

        public static void Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            XmlSerialize<PlayerSettings>.Save(fileName, Settings);
        }

        /* Popup settings dialog */
        public static DialogResult ShowDialog(IWin32Window parent)
        {
            using (var settings = new FrmSettings())
            {
                return settings.ShowDialog(parent);
            }
        }
    }
}
