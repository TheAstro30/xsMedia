/* xsMedia - xsSettings
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using xsCore.Controls.Forms;
using xsCore.Internal;
using xsCore.Settings.Data.History;
using xsCore.Utils.IO;
using xsCore.Utils.Serialization;

namespace xsCore
{
    public static class SettingsManager
    {
        /* Max constants - easier to find and change at top of file */
        private const int HistoryMaxEntries = 25;
        private const int FavoriteMaxEntries = 1000;

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
            /* First check it doesn't already exist in the list - reorder to top if it does */
            if (BringHistoryItemToTop(fileName))
            {
                return;
            }
            /* Now add it */
            var data = new HistoryData
            {
                FilePath = fileName,
                FriendlyName = Path.GetFileNameWithoutExtension(fileName)
            };
            var historyData = Settings.Player.History;
            historyData.HistoryData.Insert(0, data);
            if (historyData.HistoryData.Count - 1 >= HistoryMaxEntries)
            {
                /* Remove last entry */
                historyData.HistoryData.RemoveAt(historyData.HistoryData.Count - 1);
            }
        }

        public static bool BringHistoryItemToTop(string fileName)
        {
            var history = Settings.Player.History;
            var data = GetHistoryItem(history.HistoryData, fileName);
            if (data == null)
            {
                return false;
            }
            history.HistoryData.Remove(data);
            history.HistoryData.Insert(0, data);
            return true;
        }

        public static int AddFavorite(HistoryData data)
        {
            /* First check this is even a legal file; CD/DVD/BluRay discs are not */
            if (!FileFilters.IsValidMediaFile(data.FilePath))
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
            if (favorite.Count -1 >= FavoriteMaxEntries)
            {
                favorite.RemoveAt(0);
            }
            return favorite.Count;
        }

        public static bool RemoveFavorite(HistoryData data)
        {            
            return Settings.Favorites.Favorite.Remove(data);
        }

        /* Check if item exists method */
        public static HistoryData GetHistoryItem(IEnumerable<HistoryData> historyData, string fileName)
        {
            return historyData.FirstOrDefault(h => h.FilePath.Equals(fileName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
