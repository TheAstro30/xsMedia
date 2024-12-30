/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.IO;
using System.Windows.Forms;
using xsCore.CdUtils;
using xsCore.Utils.IO;
using xsMedia.Forms;
using xsPlaylist;
using xsSettings;
using xsVlc.Common;

namespace xsMedia.Logic
{
    public sealed class Open
    {
        private static FrmPlayer _player;

        public static string ClipFile { get; set; }
        public static string DriveLetter { get; set; }
        public static string DriveLabel { get; set; }

        public static void Init(FrmPlayer player)
        {
            _player = player;            
        }

        public static void OpenFile(bool multi = false)
        {
            var path = SettingsManager.Settings.Paths.GetPath("open-file");
            using (var ofd = new OpenFileDialog
                                 {
                                     Title = @"Open Media File...",
                                     InitialDirectory = path.Location,
                                     Multiselect = multi,
                                     Filter = Filters.OpenFilters.ToString()
                                 })
            {
                if (ofd.ShowDialog(_player) != DialogResult.OK)
                {
                    return;
                }
                path.Location = Path.GetDirectoryName(ofd.FileName);
                Video.VideoControl.OpenDiscType = DiscType.None;
                Player.IsVideoWindowInit = false;
                Media.MediaBarControl.Position = 0;
                Media.MediaBarControl.ElapsedTime = 0;
                Video.KeepVideoSize = false;
                if (Video.VideoControl.PlayerState == MediaState.Playing || Video.VideoControl.PlayerState == MediaState.Paused)
                {
                    Player.StopClip(false);
                }
                if (multi)
                {
                    foreach (var file in ofd.FileNames)
                    {
                        Video.VideoControl.OpenFile(file, false);
                    }
                    /* Play last entry */
                    if (PlaylistManager.Playlist.Count != 0)
                    {
                        Video.VideoControl.Play(PlaylistManager.Playlist.Count - 1);
                    }
                }
                else
                {
                    ClipFile = Path.GetFileName(ofd.FileName);
                    Video.VideoControl.OpenFile(ofd.FileName);
                }
            }
        }

        public static void OpenNetwork()
        {
            using (var f = new FrmNetwork())
            {
                if (f.ShowDialog(_player) == DialogResult.Cancel) { return; }
                if (Video.VideoControl.PlayerState == MediaState.Playing || Video.VideoControl.PlayerState == MediaState.Paused)
                {
                    Player.StopClip(false);
                }
                Video.VideoControl.SpinnerActive = true;
                _player.Text = @"xsMedia Player - Opening";
                Video.VideoControl.OpenNetwork(f.SelectedUrl);
            }
        }

        public static void OpenFolder()
        {
            var path = SettingsManager.Settings.Paths.GetPath("open-folder");
            using (var fbd = new FolderBrowserDialog
                              {
                                  SelectedPath = path.Location,
                                  Description = @"Select a media folder to add:",
                                  ShowNewFolderButton = false
                              })
            {
                if (fbd.ShowDialog(_player) == DialogResult.Cancel)
                {
                    return;
                }
                path.Location = fbd.SelectedPath;
                Video.VideoControl.OpenFolder(path.Location);
            }
        }

        public static void OpenDisc(int device)
        {
            var drive = Video.VideoControl.CdControl.AvailableDrives[device];
            DriveLetter = drive.DriveLabel.Replace(@"\", null);
            DriveLabel = drive.VolumeLabel;
            Player.IsVideoWindowInit = false;
            Media.MediaBarControl.Position = 0;
            Media.MediaBarControl.ElapsedTime = 0;
            if (Video.VideoControl.PlayerState == MediaState.Playing || Video.VideoControl.PlayerState == MediaState.Paused)
            {
                Player.StopClip(false);
            }
            if (!drive.IsReady)
            {
                return;
            }
            Video.VideoControl.OpenDiscType = drive.Type;
            if (drive.Type != DiscType.CdRom)
            {
                Video.VideoControl.OpenDisc(DriveLetter);
            }
            else
            {
                /* Disc is CD/DVD Rom - attempt to open as a folder ... */
                Video.VideoControl.OpenFolder(DriveLetter);
            }
        }

        public static void OpenList()
        {
            var path = SettingsManager.Settings.Paths.GetPath("open-list");
            using (var ofd = new OpenFileDialog
                                 {
                                     Title = @"Select a supported playlist file to load",
                                     InitialDirectory = path.Location,
                                     Filter = Filters.OpenPlaylistFilters.ToString()
                                 })
            {
                if (ofd.ShowDialog(_player) == DialogResult.Cancel)
                {
                    return;
                }
                path.Location = Path.GetDirectoryName(ofd.FileName);
                if (Video.VideoControl.PlayerState == MediaState.Playing || Video.VideoControl.PlayerState == MediaState.Paused)
                {
                    Player.StopClip(false);
                }
                Video.VideoControl.OpenList(ofd.FileName);
            }
        }

        public static void SaveList()
        {
            if (PlaylistManager.Playlist.Count == 0)
            {
                return;
            }
            var path = SettingsManager.Settings.Paths.GetPath("save-list");
            using (var sfd = new SaveFileDialog
                                 {
                                     Title = @"Select a playlist file to save to",
                                     InitialDirectory = path.Location,
                                     Filter = Filters.SavePlaylistFilters.ToString()
                                 })
            {
                if (sfd.ShowDialog(_player) == DialogResult.Cancel)
                {
                    return;
                }
                path.Location = Path.GetDirectoryName(sfd.FileName);
                PlaylistManager.Save(sfd.FileName);
            }
        }
    }
}
