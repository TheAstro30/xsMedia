/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xsCore.Utils.IO;
using xsCore.Utils.Serialization;
using xsSettings.Forms;
using xsSettings.Internal;
using xsSettings.Settings;

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
            /* Check some important variables are set */
            if (Settings.Player.JumpStep == 0)
            {
                Settings.Player.JumpStep = 5;
            }
            if (Settings.Player.VolumeStep == 0)
            {
                Settings.Player.VolumeStep = 5;
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

        /* History/favorites adding/removal methods */
        public static void AddHistory(string fileName)
        {
            AddToList(Settings.Player.FileHistory, fileName, 25);
        }

        public static int AddFavorite(SettingsHistoryData data)
        {
            /* First check this is even a legal file; CD/DVD/Blueray discs are not */
            if (!Filters.IsValidMediaFile(data.FilePath))
            {
                return -1;
            }
            /* First check it doesn't exist */
            var favorite = Settings.Favorites.Favorite;
            if (GetHistoryItem(favorite, data.FilePath) != null)
            {
                return -1;
            }
            favorite.Add(data);
            if (favorite.Count -1 > 1000)
            {
                favorite.RemoveAt(0);
            }
            return favorite.Count;
        }

        public static bool RemoveFavorite(SettingsHistoryData data)
        {            
            return Settings.Favorites.Favorite.Remove(data);
        }

        /* Check if item exists method */
        public static SettingsHistoryData GetHistoryItem(IEnumerable<SettingsHistoryData> historyData, string fileName)
        {
            return historyData.FirstOrDefault(h => h.FilePath.Equals(fileName, StringComparison.InvariantCultureIgnoreCase));
        }

        /* Private add method */
        private static void AddToList(IList<SettingsHistoryData> historyData, string fileName, int maxEnties, bool append = false)
        {
            /* First check it doesn't already exist in the list */
            if (GetHistoryItem(historyData, fileName) != null)
            {
                return;
            }
            /* Now add it */
            var data = new SettingsHistoryData
            {
                FilePath = fileName,
                FriendlyName = Path.GetFileNameWithoutExtension(fileName)
            };
            if (append)
            {
                historyData.Add(data);
            }
            else
            {
                historyData.Insert(0, data);
            }
            if (historyData.Count - 1 > maxEnties)
            {
                /* Remove last entry */
                historyData.RemoveAt(!append ? historyData.Count - 1 : 0);
            }
        }
    }
}
