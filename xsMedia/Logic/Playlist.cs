﻿/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Collections.Generic;
using System.Drawing;
using xsCore.CdUtils;
using xsMedia.Forms;
using xsPlaylist;
using xsPlaylist.Controls;
using xsPlaylist.Playlist;
using xsPlaylist.Utils;
using xsVlc.Common;

namespace xsMedia.Logic
{
    public sealed class Playlist
    {
        private static FrmPlayer _player;

        public static PlaylistListView PlaylistControl { get; set; }

        public static void Init(FrmPlayer player)
        {
            _player = player;
            PlaylistControl = new PlaylistListView
                                          {
                                              Font = _player.Font,
                                              Visible = false
                                          };
            _player.Controls.Add(PlaylistControl);
            /* Events */
            PlaylistManager.PlaylistEntriesChanged += OnPlaylistEntriesChanged;
            PlaylistManager.PlaylistItemAdded += OnPlaylistItemAdded;
            PlaylistManager.PlaylistItemRemoved += OnPlaylistItemRemoved;
            PlaylistManager.PlaylistMetaDataChanged += OnPlaylistMetaDataChanged;
            PlaylistControl.OnItemSelected += OnPlaylistDoubleClick;
            PlaylistControl.OnListRightClick += OnPlaylistRightClick;
            PlaylistControl.OnItemRightClick += OnPlaylistItemRightClick;
        }

        /* Update playlist meta data methods */
        public static void UpdatePlaylistMeta()
        {
            UpdatePlaylistMeta((int)Video.VideoControl.Length, Video.VideoControl.CurrentMedia.IsParsed);
        }

        public static void UpdatePlaylistMeta(int duration, bool parsed)
        {
            var index = Video.VideoControl.CurrentTrack;
            var meta = PlaylistManager.GetMetaData(index);
            if (meta == null) { return; }
            UpdatePlaylistMeta(meta, index, duration, parsed);
        }

        public static void UpdatePlaylistMeta(PlaylistEntry entry, int index, int duration, bool parsed)
        {
            /* Update playlist meta data */
            if (duration != 0) { entry.Length = duration; }
            if (parsed)
            {
                switch (Video.VideoControl.OpenDiscType)
                {
                    case DiscType.Vcd:
                        entry.Title = string.Format("VCD {0} {1}", Open.DriveLetter, Open.DriveLabel);
                        break;

                    case DiscType.Dvd:
                        entry.Title = string.Format("DVD {0} {1}", Open.DriveLetter, Open.DriveLabel);
                        break;

                    default:
                        if (string.IsNullOrEmpty(entry.Title))
                        {
                            entry.Title = Video.VideoControl.CurrentMedia.GetMetaData(MetaDataType.Title);
                        }
                        if (string.IsNullOrEmpty(entry.Artist))
                        {
                            entry.Artist = Video.VideoControl.CurrentMedia.GetMetaData(MetaDataType.Artist);
                        }
                        if (string.IsNullOrEmpty(entry.Album))
                        {
                            entry.Album = Video.VideoControl.CurrentMedia.GetMetaData(MetaDataType.Album);
                        }
                        /* Get album art - have to do it the long way */
                        var bmp = AlbumArt.GetAlbumArt(entry.Location, entry.Artist, entry.Album);
                        if (bmp != null)
                        {
                            Video.VideoControl.LogoImage = new Bitmap(bmp);
                            Video.VideoControl.LogoImageMaximumSize = Video.VideoControl.LogoImage.Size;
                            Video.VideoControl.Refresh();
                        }
                        break;
                }
            }
            /* Update changes */
            PlaylistManager.SetMetaData(index, entry);
        }

        /* Callbacks/events */
        private static void OnPlaylistEntriesChanged()
        {
            if (PlaylistControl.InvokeRequired)
            {
                Player.Sync.Execute(OnPlaylistEntriesChanged);
                return;
            }
            if (PlaylistManager.MediaList.Count != 0)
            {
                Video.VideoControl.CurrentTrack = 0;
                Video.VideoControl.Play(Video.VideoControl.CurrentTrack);
            }
            else
            {
                /* Playlist is now empty, stop playback */
                Player.StopClip(false);
            }
            /* Update list view */
            PlaylistControl.Clear();
            foreach (var entry in PlaylistManager.Playlist)
            {
                PlaylistControl.Add(entry);
            }
        }

        private static void OnPlaylistItemAdded(PlaylistEntry entry)
        {
            if (PlaylistControl.InvokeRequired)
            {
                Player.Sync.Execute(() => OnPlaylistItemAdded(entry));
                return;
            }
            PlaylistControl.Add(entry);
        }

        private static void OnPlaylistItemRemoved(IList<int> entries)
        {
            if (PlaylistControl.InvokeRequired)
            {
                Player.Sync.Execute(() => OnPlaylistItemRemoved(entries));
                return;
            }
            if (PlaylistManager.MediaList.Count != 0)
            {
                Video.VideoControl.CurrentTrack = 0;
                Video.VideoControl.Play(Video.VideoControl.CurrentTrack);
            }
            else
            {
                /* Playlist is now empty, stop playback */
                Player.StopClip(false);
            }
            /* Update playlist */
            for (var index = entries.Count - 1; index >= 0; --index)
            {
                PlaylistControl.RemoveAt(entries[index]);
            }
        }

        private static void OnPlaylistMetaDataChanged(int index, PlaylistEntry entry)
        {
            if (PlaylistControl.InvokeRequired)
            {
                Player.Sync.Execute(() => OnPlaylistMetaDataChanged(index, entry));
                return;
            }            
            PlaylistControl.Update(entry);
            PlaylistControl.SelectedIndex = Video.VideoControl.CurrentTrack;
        }

        private static void OnPlaylistDoubleClick(int index)
        {
            Video.VideoControl.OpenDiscType = DiscType.None;
            Player.IsVideoWindowInit = false;
            Media.MediaBarControl.Position = 0;
            Media.MediaBarControl.ElapsedTime = 0;
            Video.KeepVideoSize = false;
            Video.VideoControl.Play(index);
        }

        private static void OnPlaylistRightClick(Point location)
        {
            Menus.MenuPlaylist.Show(PlaylistControl, location);
        }

        private static void OnPlaylistItemRightClick(Point location)
        {
            Menus.MenuPlaylistItem.Show(PlaylistControl, location);
        }
    }
}
