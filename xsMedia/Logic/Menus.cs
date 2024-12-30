/* xsMedia - Media Player
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using xsCore;
using xsCore.CdUtils;
using xsCore.Controls.Forms;
using xsCore.Controls.Playlist.Playlist;
using xsCore.Settings.Data;
using xsCore.Settings.Data.Enums;
using xsCore.Skin;
using xsCore.Utils;
using xsCore.Utils.IO;
using xsCore.Utils.SystemUtils;
using xsCore.Utils.UI;
using xsMedia.Forms;
using xsMedia.Properties;
using xsVlc.Common;
using xsVlc.Common.Events;

namespace xsMedia.Logic
{
    /* Static class for menu construction (keeps code minimal on form - even though this file is big) */
    public sealed class Menus
    {
        private static FrmPlayer _player;
        private static FolderSearch _skinsSearch;

        public static MenuStrip MainMenu { get; set; }

        public static bool IsControlHovering { get; private set; }

        public static ToolStripMenuItem MenuMedia { get; private set; }
        public static ToolStripMenuItem MenuPlayback { get; private set; }
        public static ToolStripMenuItem MenuAudio { get; private set; }
        public static ToolStripMenuItem MenuVideo { get; private set; }
        public static ToolStripMenuItem MenuSubtitles { get; private set; }
        public static ToolStripMenuItem MenuPrefs { get; private set; }
        public static ToolStripMenuItem MenuHelp { get; private set; }

        public static ToolStripMenuItem MenuFull { get; private set; }
        public static ToolStripMenuItem MenuList { get; private set; }
        public static ToolStripMenuItem MenuClose { get; private set; }

        public static ContextMenuStrip MenuOpen { get; private set; }
        public static ContextMenuStrip MenuVideoWindow { get; private set; }

        public static ContextMenuStrip MenuPlaylist { get; private set; }
        public static ContextMenuStrip MenuPlaylistItem { get; private set; }

        #region Init(FrmPlayer player)
        public static void Init(FrmPlayer player)
        {
            /* This file is pretty big and complicated... */
            _player = player;

            Media.MediaBarControl.MouseEnter += OnControlMouseEnter;
            Media.MediaBarControl.MouseLeave += OnControlMouseLeave;

            Video.VideoControl.MediaStateChanged += OnMediaStateChanged;

            MainMenu = new MenuStrip
            {
                Font = _player.Font,
                ShowItemToolTips = true,                
                Dock = DockStyle.Top
            };
            MainMenu.MouseEnter += OnControlMouseEnter;
            MainMenu.MouseLeave += OnControlMouseLeave;

            MenuMedia = new ToolStripMenuItem
            {
                Text = @"Media",
                Tag = "ROOT"
            };
            MenuMedia.DropDownOpening += OnOpenMenuOpening;
            MenuMedia.DropDown.MouseEnter += OnControlMouseEnter;
            MenuMedia.DropDown.MouseLeave += OnControlMouseLeave;

            MenuPlayback = new ToolStripMenuItem
            {
                Text = @"Playback",
                Tag = "ROOT"
            };
            MenuPlayback.DropDownOpening += OnPlaybackMenuOpening;
            MenuPlayback.DropDown.MouseEnter += OnControlMouseEnter;
            MenuPlayback.DropDown.MouseLeave += OnControlMouseLeave;

            MenuAudio = new ToolStripMenuItem
            {
                Text = @"Audio",
                Tag = "ROOT"
            };
            MenuAudio.DropDownOpening += OnAudioMenuOpening;
            MenuAudio.DropDown.MouseEnter += OnControlMouseEnter;
            MenuAudio.MouseLeave += OnControlMouseLeave;

            MenuVideo = new ToolStripMenuItem
            {
                Text = @"Video",
                Tag = "ROOT"
            };
            MenuVideo.DropDownOpening += OnVideoMenuOpening;
            MenuVideo.DropDown.MouseEnter += OnControlMouseEnter;
            MenuVideo.DropDown.MouseLeave += OnControlMouseLeave;

            MenuSubtitles = new ToolStripMenuItem
            {
                Text = @"Subtitles",
                Tag = "ROOT"
            };
            MenuSubtitles.DropDownOpening += OnSubtitlesMenuOpening;
            MenuSubtitles.DropDown.MouseEnter += OnControlMouseEnter;
            MenuSubtitles.DropDown.MouseLeave += OnControlMouseLeave;

            MenuPrefs = new ToolStripMenuItem
            {
                Text = @"Preferences",
                Tag = "ROOT"
            };
            MenuPrefs.DropDownOpening += OnPrefsMenuOpening;
            MenuPrefs.DropDown.MouseEnter += OnControlMouseEnter;
            MenuPrefs.DropDown.MouseLeave += OnControlMouseLeave;

            MenuHelp = new ToolStripMenuItem
            {
                Text = @"Help",
                Tag = "ROOT"
            };
            MenuHelp.DropDown.MouseEnter += OnControlMouseEnter;
            MenuHelp.DropDown.MouseLeave += OnControlMouseLeave;

            MenuFull = new ToolStripMenuItem
            {
                Font = new Font("Wingdings 3", 7),
                Text = @"{",
                Alignment = ToolStripItemAlignment.Right,
                ToolTipText = @"Enter full screen mode",
                Tag = "ROOT"
            };
            MenuFull.Click += Video.OnVideoFullscreen;
            
            MenuList = new ToolStripMenuItem
            {
                Font = new Font("Wingdings 3", 11),
                Text = @")",
                Alignment = ToolStripItemAlignment.Right,
                ToolTipText = @"Show playlist",
                Tag = "ROOT"
            };
            MenuList.Click += OnMenuListClicked;

            MenuClose = new ToolStripMenuItem
            {
                Font = new Font("Wingdings 2", 11),
                Text = @"O",
                Alignment = ToolStripItemAlignment.Right,
                ToolTipText = @"Close xsMedia",
                Tag = "ROOT",
                Visible = false
            };
            MenuClose.Click += OnMenuCloseClicked;

            MainMenu.Items.AddRange(new ToolStripItem[]
            {
                MenuMedia, MenuPlayback, MenuAudio, MenuVideo, MenuSubtitles, MenuPrefs, MenuHelp, MenuClose, MenuList, MenuFull
            });
            _player.Controls.Add(MainMenu);

            /* Context menus */
            MenuOpen = new ContextMenuStrip();
            MenuOpen.Opening += OnOpenMenuOpening;
            MenuOpen.MouseEnter += OnControlMouseEnter;
            MenuOpen.MouseLeave += OnControlMouseLeave;

            MenuVideoWindow = new ContextMenuStrip();
            MenuVideoWindow.Opening += OnVideoWindowMenuOpening;
            MenuVideoWindow.MouseEnter += OnControlMouseEnter;
            MenuVideoWindow.MouseLeave += OnControlMouseLeave;

            MenuPlaylist = new ContextMenuStrip();
            BuildPlaylistMenu(MenuPlaylist, OnPlaylistMenuItemClicked);

            MenuPlaylistItem = new ContextMenuStrip();
            BuildMenuPlaylistItem(MenuPlaylistItem, OnPlaylistMenuItemClicked);
            MenuPlaylistItem.Opening += OnPlaylistItemMenuOpening;

            BuildMenuMedia(MenuMedia, OnOpenMenuItemClicked, Video.VideoControl.CdControl);
            BuildOpenMenu(MenuOpen, OnOpenMenuItemClicked, Video.VideoControl.CdControl);
            BuildMenuPlayback(MenuPlayback, OnPlaybackMenuItemClicked);
            BuildMenuVideo(MenuVideo, OnVideoMenuItemClicked);
            BuildMenuHelp(MenuHelp, OnHelpMenuItemClicked);
            BuildVideoWindowMenu();

            /* Search for available skins */
            _skinsSearch = new FolderSearch();
            _skinsSearch.BeginSearch(new DirectoryInfo(AppPath.MainDir(@"\skins", false)), "*.xml", "*", true);
            _skinsSearch.OnFileFound += OnSkinFileFound;
            _skinsSearch.OnFileSearchCompleted += OnSkinFileComplete;
        }
        #endregion

        #region Methods
        public static void ShowVideoMenu(Point location)
        {
            if (Playlist.PlaylistControl.Visible)
            {
                return;
            }
            MenuVideoWindow.Show(location);
        }

        private static void UpdateMenus()
        {
            /* This is only done so hot keys work from first play */
            BuildMenuPlayback(MenuPlayback, OnPlaybackMenuItemClicked);
            BuildMenuVideo(MenuVideo, OnVideoMenuItemClicked);
            IsControlHovering = false;
        }
        #endregion

        #region Callbacks
        #region Menu drop down opening
        /* Open menu */
        private static void OnOpenMenuOpening(object sender, EventArgs e)
        {
            BuildOpenMenu(MenuOpen, OnOpenMenuItemClicked, Video.VideoControl.CdControl);
            BuildMenuMedia(MenuMedia, OnOpenMenuItemClicked, Video.VideoControl.CdControl);
        }

        /* Playback menu */
        private static void OnPlaybackMenuOpening(object sender, EventArgs e)
        {
            BuildMenuPlayback(MenuPlayback, OnPlaybackMenuItemClicked);
        }

        /* Audio menu */
        private static void OnAudioMenuOpening(object sender, EventArgs e)
        {
            BuildMenuAudio(MenuAudio, OnAudioMenuItemClicked);
        }

        /* Video menu */
        private static void OnVideoMenuOpening(object sender, EventArgs e)
        {
            BuildMenuVideo(MenuVideo, OnVideoMenuItemClicked);
        }

        /* Subtitles menu */
        private static void OnSubtitlesMenuOpening(object sender, EventArgs e)
        {
            BuildMenuSubtitles(MenuSubtitles, OnSubtitlesMenuItemClicked);
        }

        /* Prefs menu */
        private static void OnPrefsMenuOpening(object sender, EventArgs e)
        {
            BuildMenuPrefs(MenuPrefs, OnPrefsMenuItemClicked);
        }

        /* Video window menu */
        private static void OnVideoWindowMenuOpening(object sender, EventArgs e)
        {
            BuildVideoWindowMenu();
        }

        /* Playlist menu */
        private static void OnPlaylistItemMenuOpening(object sender, EventArgs e)
        {
            BuildMenuPlaylistItem(MenuPlaylistItem, OnPlaylistMenuItemClicked);
        }
        #endregion

        #region Menu item click callbacks
        #region Menu bar
        /* Playlist menu click */
        public static void OnMenuListClicked(object sender, EventArgs e)
        {
            /* Switch list visiblity */
            Video.VideoControl.Visible = !Video.VideoControl.Visible;
            Playlist.PlaylistControl.Visible = !Playlist.PlaylistControl.Visible;
            if (Playlist.PlaylistControl.Visible)
            {
                Playlist.PlaylistControl.Focus();
            }
            /* Change "arrow" direction */
            MenuList.Text = Video.VideoControl.Visible ? ")" : "*";
            MenuList.ToolTipText = Video.VideoControl.Visible ? "Show playlist" : "Hide playlist";
        }

        /* Toggle full screen */
        private static void OnVideoWindowFullScreenMenuClick(object sender, EventArgs e)
        {
            Video.OnVideoFullscreen(sender, e);
        }

        /* Close button when in full screen mode */
        private static void OnMenuCloseClicked(object sender, EventArgs e)
        {
            _player.Close();
        }
        #endregion

        #region Media/Open menu
        private static void OnOpenMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripItem)sender;
            switch (o.Tag.ToString())
            {
                case "OPEN":
                    Open.OpenFile();
                    break;

                case "NET":
                    Open.OpenNetwork();
                    break;

                case "LIST":
                    Open.OpenList();
                    break;

                case "FOLDER":
                    Open.OpenFolder();
                    break;

                case "SAVE":
                    Open.SaveList();
                    break;

                case "CLOSE":
                    _player.Close();
                    break;

                default:
                    /* CD Device */
                    Open.OpenDisc(Convert.ToInt32(o.Tag));
                    break;
            }
        }

        private static void OnFavoritesMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripItem)sender;
            var tag = o.Tag.ToString();
            switch (tag)
            {
                case "MANAGE":
                    var fave = new FrmFavorites();
                    if (FormManager.Open(fave, _player) == null)
                    {
                        System.Diagnostics.Debug.Print("is null");
                    }
                    break;

                default:
                    /* Begin playback */
                    Video.VideoControl.OpenFile(tag);
                    break;
            }
        }

        private static void OnRecentMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripItem)sender;
            var tag = o.Tag.ToString();
            switch (tag)
            {
                case "CLEAR":
                    SettingsManager.Settings.Player.FileHistory.Clear();
                    break;

                default:
                    /* File name */
                    Video.VideoControl.OpenDiscType = DiscType.None;
                    Player.IsVideoWindowInit = false;
                    Media.MediaBarControl.Position = 0;
                    Media.MediaBarControl.ElapsedTime = 0;
                    Video.KeepVideoSize = false;
                    Video.VideoControl.OpenFile(tag);
                    break;
            }
        }
        #endregion

        #region Playback menu
        private static void OnPlaybackMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            switch (o.Tag.ToString())
            {
                case "JUMPBACK":
                    Video.OnVideoKeyDown(sender, new KeyEventArgs(Keys.Left));
                    break;

                case "JUMPFORWARD":
                    Video.OnVideoKeyDown(sender, new KeyEventArgs(Keys.Right));
                    break;

                case "JUMPTIME":
                    if (Video.VideoControl.PlayerState != MediaState.Playing && Video.VideoControl.PlayerState != MediaState.Paused)
                    {
                        /* Just to make sure */
                        return;
                    }
                    var jump = new FrmGoto
                    {
                        MediaLength = Video.VideoControl.Length,
                        MediaCurrentPosition = Video.VideoControl.Position
                    };
                    jump.ShowDialog(_player);
                    if (jump.DialogResult == DialogResult.OK)
                    {
                        Video.VideoControl.Position = jump.MediaNewPosition;
                        Media.MediaBarControl.Position = jump.MediaNewPosition * 100;
                        Media.MediaBarControl.ElapsedTime = (int)(jump.MediaNewPosition * Video.VideoControl.Length) / 1000;
                    }
                    break;

                case "PLAY":
                    if (Video.VideoControl.PlayerState == MediaState.Paused)
                    {
                        Video.VideoControl.Play();
                    }
                    else
                    {
                        /* Nothing is playing, so we need to open a file */
                        Open.OpenFile();
                    }
                    break;

                case "PAUSE":
                    Video.VideoControl.Pause();
                    break;

                case "STOP":
                    Video.VideoControl.Stop();
                    break;
            }
        }

        private static void OnPlaybackSpeedItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            float rate;
            if (!float.TryParse(o.Tag.ToString(), out rate))
            {
                return;
            }
            Video.VideoControl.PlaybackSpeed = rate;
            SettingsManager.Settings.Player.Speed = rate;
        }
        #endregion

        #region Audio menu
        private static void OnAudioMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            var tag = o.Tag.ToString();
            switch (tag)
            {
                case "DISABLE":
                    Video.VideoControl.AudioTrack = -1;
                    break;

                case "MUTE":
                    Video.VideoControl.ToggleMute();
                    break;

                case "EFFECTS":
                    var f = new FrmEffects("audio", null, null);
                    var ret = FormManager.Open(f, _player);
                    if (ret != f)
                    {
                        /* Already open */
                        ((FrmEffects)ret).SelectTabs("audio");
                        f.Dispose();
                    }
                    break;

                default:
                    int track;
                    if (int.TryParse(tag, out track))
                    {
                        Video.VideoControl.AudioTrack = track;
                    }
                    break;
            }
        }
        #endregion

        #region Video menu
        private static void OnVideoMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            var tag = o.Tag.ToString();
            switch (tag)
            {
                case "ADD":
                    var p = Playlist.PlaylistControl.GetItemAt(Video.VideoControl.CurrentTrack);
                    if (p == null)
                    {
                        return;
                    }
                    var data = new HistoryData(p.Location, p.Length / 1000);
                    var i = SettingsManager.AddFavorite(data);
                    if (i == -1)
                    {
                        /* Already exists */
                        return;
                    }
                    var fave = FormManager.GetForm("FrmFavorites");
                    if (fave != null)
                    {
                        ((FrmFavorites)fave).AddFavorite(data);
                    }
                    break;

                case "DISABLE":
                    Video.VideoControl.VideoTrack = -1;
                    Video.VideoControl.IsVideo = false;
                    Video.VideoControl.Refresh();
                    break;

                case "EFFECTS":
                    var f = new FrmEffects("video", null, null);
                    var ret = FormManager.Open(f, _player);
                    if (ret != f)
                    {
                        /* Already open */
                        ((FrmEffects)ret).SelectTabs("video");
                    }
                    break;

                case "SNAPSHOT":
                    Video.VideoControl.TakeSnapShot();
                    break;

                default:
                    int track;
                    var aspect = tag.Split('-');
                    if (aspect.Length > 1)
                    {
                        switch (aspect[0])
                        {
                            case "ASPECT":
                                if (int.TryParse(aspect[1], out track))
                                {
                                    Video.VideoControl.AspectRatio = (AspectRatioMode)track;
                                }
                                break;

                            case "ZOOM":
                                if (int.TryParse(aspect[1], out track))
                                {
                                    Video.VideoControl.ZoomRatio = (ZoomRatioMode)track;
                                    Video.KeepVideoSize = false;
                                    Player.InitVideoWindow(Video.VideoControl.ZoomRatio);
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (int.TryParse(tag, out track))
                        {
                            Video.VideoControl.VideoTrack = track;
                            Video.VideoControl.IsVideo = true;
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Subtitles menu
        private static void OnSubtitlesMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            var tag = o.Tag.ToString();
            switch (tag)
            {
                case "ADD":
                    if (!Video.VideoControl.IsVideo)
                    {
                        return;
                    }
                    var path = SettingsManager.Settings.Paths.GetPath("open-sub");
                    using (var ofd = new OpenFileDialog
                    {
                        Title = @"Open Subtitle File",
                        InitialDirectory = path.Location,
                        Multiselect = false,
                        Filter = FileFilters.SubtitleFilters.ToString()
                    })
                    {
                        if (ofd.ShowDialog(_player) != DialogResult.OK)
                        {
                            return;
                        }
                        path.Location = Path.GetDirectoryName(ofd.FileName);
                        Video.VideoControl.SetSubtitleFile = ofd.FileName;
                    }
                    break;

                case "DISABLE":
                    Video.VideoControl.Subtitles = -1;
                    break;

                default:
                    int sub;
                    if (int.TryParse(tag, out sub))
                    {
                        Video.VideoControl.Subtitles = sub;
                    }
                    break;
            }
        }
        #endregion

        #region Preferences menu
        private static void OnPrefsMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            var tag = o.Tag.ToString();
            switch (tag)
            {
                case "EFFECTS":
                    var effects = new FrmEffects();
                    FormManager.Open(effects, _player);
                    break;

                case "OPTIONS":
                    var d = SettingsManager.ShowDialog(_player);
                    if (d == DialogResult.OK)
                    {
                        Media.MediaBarControl.Counter.CounterType = SettingsManager.Settings.Player.CounterType;
                        Video.VideoControl.PlaybackSpeed = SettingsManager.Settings.Player.Speed;
                    }
                    break;

                default:
                    if (tag.StartsWith("SKIN-"))
                    {
                        var skin = tag.Split('-');
                        int index;
                        if (skin.Length > 1 && int.TryParse(skin[1], out index))
                        {
                            Media.MediaBarControl.LoadSkin(SkinManager.AvailableSkins[index].Path);
                            Playlist.PlaylistControl.SkinChanged(); /* Make sure listview of playlist also gets the new skin */
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Help menu
        private static void OnHelpMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            switch (o.Tag.ToString())
            {
                case "ABOUT":
                    using (var f = new FrmAbout())
                    {
                        f.ShowDialog(_player);
                    }
                    break;
                case "VLC":
                    ExecuteProcess.BeginProcess("http://www.videolan.org/vlc/libvlc.html");
                    break;
            }
        }
        #endregion

        #region Playlist window context menu
        private static void OnPlaylistMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            switch (o.Tag.ToString())
            {
                case "NEW":
                    PlaylistManager.Clear();
                    break;

                case "OPEN":
                    Open.OpenList();
                    break;

                case "SAVE":
                    Open.SaveList();
                    break;

                case "ADDFILES":
                    Open.OpenFile(true);
                    break;

                case "ADDFOLDER":
                    Open.OpenFolder();
                    break;

                case "SORTTITLE":
                    PlaylistManager.Sort(PlaylistSortType.Title);
                    break;

                case "SORTARTIST":
                    PlaylistManager.Sort(PlaylistSortType.Artist);
                    break;

                case "SORTLOCATION":
                    PlaylistManager.Sort(PlaylistSortType.Path);
                    break;

                case "SORTRANDOM":
                    PlaylistManager.Sort(PlaylistSortType.Random);
                    break;

                case "REMOVEDEAD":
                    PlaylistManager.RemoveDead();
                    break;

                case "REMOVEDUP":
                    PlaylistManager.RemoveDuplicates();
                    break;

                case "REMOVE":
                    var l = Playlist.PlaylistControl.SelectedIndices.Cast<int>().ToList();
                    PlaylistManager.Remove(l);
                    break;

                case "INFO":
                    var index = Playlist.PlaylistControl.SelectedIndex;
                    if (index == -1) { return; }
                    using (var meta = new FrmMediaMeta(PlaylistManager.Playlist[index].Location))
                    {
                        meta.ShowDialog(_player);
                    }
                    break;
            }
        }
        #endregion
        #endregion

        #region Control callbacks
        private static void OnControlMouseEnter(object sender, EventArgs e)
        {
            IsControlHovering = true;
        }

        private static void OnControlMouseLeave(object sender, EventArgs e)
        {
            IsControlHovering = false;
        }

        private static void OnMediaStateChanged(object sender, MediaStateChange e)
        {
            switch (e.NewState)
            {
                case MediaState.Playing:
                    /* This is only done so hot keys work from first play */
                    Player.Sync.Execute(UpdateMenus);
                    break;
            }
        }

        /* Skin file callback */
        private static void OnSkinFileFound(string fileName)
        {
            var skinName = "Unknown";
            var skin = new XmlDocument();
            /* Load XML data */
            skin.Load(fileName);
            var skinData = skin.SelectNodes("skin/head");
            if (skinData != null)
            {
                var data = skinData.Item(0);
                if (data != null)
                {
                    var attrib = data.Attributes;
                    skinName = attrib != null && attrib.Count > 1
                                   ? string.Format("{0} ({1})", attrib.Item(0).Value, attrib.Item(1).Value)
                                   : Path.GetFileNameWithoutExtension(fileName);
                }
            }
            /* Add skin info */
            var info = new AvailableSkinsInfo { Name = skinName, Path = fileName };
            SkinManager.AvailableSkins.Add(info);
        }

        private static void OnSkinFileComplete(FolderSearch sender)
        {
            Player.Sync.Execute(() => BuildMenuPrefs(MenuPrefs, OnPrefsMenuItemClicked));
        }
        #endregion
        #endregion

        #region Menu builders
        #region Menu bar
        #region Media menu
        private static void BuildMenuMedia(ToolStripDropDownItem menu, EventHandler menuHandler, CdManager cdManager)
        {
            menu.DropDownItems.Clear();
            menu.DropDownItems.AddRange(OpenMenu(menuHandler, cdManager));

            menu.DropDownItems.AddRange(
                new ToolStripItem[]
                    {
                        new ToolStripSeparator(),
                        MenuHelper.AddMenuItem("Save Playlist", "SAVE", Keys.Control | Keys.S, true, false,
                                               Resources.menuSave.ToBitmap(),
                                               menuHandler),
                        new ToolStripSeparator(),
                        MenuHelper.AddMenuItem("Close", "CLOSE", Keys.Alt | Keys.F4, true, false,
                                               Resources.menuClose.ToBitmap(),
                                               menuHandler)
                    }
                );
            IsControlHovering = true;
        }
        #endregion

        #region Playback menu
        private static void BuildMenuPlayback(ToolStripDropDownItem menu, EventHandler menuHandler)
        {
            menu.DropDownItems.Clear();
            var enabled = Video.VideoControl.PlayerState == MediaState.Playing ||
                          Video.VideoControl.PlayerState == MediaState.Paused;

            var play = Video.VideoControl.PlayerState == MediaState.Playing ? "Pause" : "Play";

            var m = new ToolStripMenuItem
            {
                Text = @"Loop",
                Image = Resources.menuLoop.ToBitmap(),
                Enabled = enabled
            };
            BuildMenuPlaybackLoop(m, menuHandler);
            menu.DropDownItems.Add(m);

            m = new ToolStripMenuItem
            {
                Text = @"Speed",
                Image = Resources.menuPlaySpeed.ToBitmap(),
                Enabled = enabled
            };
            BuildMenuPlaybackSpeed(m, OnPlaybackSpeedItemClicked);

            menu.DropDownItems.AddRange(new ToolStripItem[]
            {
                m,
                new ToolStripSeparator(),
                MenuHelper.AddMenuItem("Jump backward", "JUMPBACK", Keys.None, enabled, false, Resources.menuJumpBack.ToBitmap(), menuHandler),
                MenuHelper.AddMenuItem("Jump forward", "JUMPFORWARD", Keys.None, enabled, false, Resources.menuJumpForward.ToBitmap(), menuHandler),
                MenuHelper.AddMenuItem("Go to specific time", "JUMPTIME", Keys.Control | Keys.G, enabled, false, Resources.menuJumpToTime.ToBitmap(), menuHandler),
                new ToolStripSeparator(),
                MenuHelper.AddMenuItem(play, play.ToUpper(), Keys.None,true,false, play.Equals("Play") ? Resources.menuPlay.ToBitmap() : Resources.menuPause.ToBitmap(), menuHandler),
                MenuHelper.AddMenuItem("Stop", "STOP", Keys.None, enabled, false, Resources.menuStop.ToBitmap(), menuHandler)
            });
            IsControlHovering = true;
        }

        private static void BuildMenuPlaybackLoop(ToolStripDropDownItem menuItem, EventHandler menuHandler)
        {
            //need to set a key
            foreach (var l in (PlaybackLoopMode[])Enum.GetValues(typeof(PlaybackLoopMode)))
            {
                menuItem.DropDownItems.Add(MenuHelper.AddMenuItem(EnumUtils.GetDescriptionFromEnumValue(l), "",
                    Keys.None, true, SettingsManager.Settings.Player.Loop == l, null, menuHandler));
            }
        }

        private static void BuildMenuPlaybackSpeed(ToolStripDropDownItem menuItem, EventHandler menuHandler)
        {
            var rate = SettingsManager.Settings.Player.Speed;
            menuItem.DropDownItems.AddRange(new ToolStripItem[]
            {
                MenuHelper.AddMenuItem("0.25", "0.25", Keys.None, true, rate.Equals(0.25F), null, menuHandler),
                MenuHelper.AddMenuItem("0.5", "0.5", Keys.None, true, rate.Equals(0.50F), null, menuHandler),
                MenuHelper.AddMenuItem("0.75", "0.75", Keys.None, true, rate.Equals(0.75F), null, menuHandler),
                MenuHelper.AddMenuItem("Normal", "1", Keys.None, true, rate.Equals(1), null, menuHandler),
                MenuHelper.AddMenuItem("1.25", "1.25", Keys.None, true, rate.Equals(1.25F), null, menuHandler),
                MenuHelper.AddMenuItem("1.75", "1.75", Keys.None, true, rate.Equals(1.75F), null, menuHandler),
                MenuHelper.AddMenuItem("2", "2", Keys.None, true, rate.Equals(2), null, menuHandler)
            });
        }
        #endregion

        #region Audio menu
        private static void BuildMenuAudio(ToolStripDropDownItem menu, EventHandler menuHandler, bool clear = true)
        {
            if (clear)
            {
                menu.DropDownItems.Clear();
            }
            var audioCount = Video.VideoControl.AudioTrackCount;
            var current = Video.VideoControl.AudioTrack;
            var m = MenuHelper.AddMenuItem("Audio Track", "TRACKS", Keys.None, audioCount > 0, false, Resources.menuAudioTrack.ToBitmap(), menuHandler);
            var tracks = Video.VideoControl.AudioTrackDescription;
            for (var t = 0; t <= tracks.Count - 1; ++t)
            {
                m.DropDownItems.Add(MenuHelper.AddMenuItem(tracks[t].psz_name,
                    (t == 0 ? "DISABLE" : tracks[t].i_id.ToString(CultureInfo.InvariantCulture)),
                    Keys.None, true,
                    (t == 0 && current == -1 || tracks[t].i_id == current), null,
                    menuHandler));
            }
            menu.DropDownItems.Add(m);
            var mute = Video.VideoControl.IsMuted;
            menu.DropDownItems.AddRange(new ToolStripItem[]
            {
                new ToolStripSeparator(),
                MenuHelper.AddMenuItem(mute ? "Unmute" : "Mute", "MUTE", Keys.None, true, false, mute ? Resources.menuAudioTrack.ToBitmap() : Resources.menuMute.ToBitmap(), menuHandler),
                MenuHelper.AddMenuItem("Effects", "EFFECTS", Keys.None, true, false, Resources.menuEffects.ToBitmap(), menuHandler)
            });
            IsControlHovering = true;
        }
        #endregion

        #region Video menu
        private static void BuildMenuVideo(ToolStripDropDownItem menu, EventHandler menuHandler, bool clear = true)
        {
            if (clear)
            {
                menu.DropDownItems.Clear();
            }
            var videoCount = Video.VideoControl.VideoTrackCount;
            var current = Video.VideoControl.VideoTrack;
            var m = MenuHelper.AddMenuItem("Video Track", "TRACKS", Keys.None, videoCount > 0, false, Resources.menuVideoTrack.ToBitmap(), menuHandler);
            var tracks = Video.VideoControl.VideoTrackDescription;
            for (var t = 0; t <= tracks.Count - 1; ++t)
            {
                m.DropDownItems.Add(MenuHelper.AddMenuItem(tracks[t].psz_name,
                                                           (t == 0 ? "DISABLE" : tracks[t].i_id.ToString(CultureInfo.InvariantCulture)),
                                                           Keys.None, true,
                                                           (t == 0 && current == -1 || tracks[t].i_id == current), null,
                                                           menuHandler));
            }
            menu.DropDownItems.AddRange(new ToolStripItem[]
                                            {
                                                m,
                                                new ToolStripSeparator()
                                            });
            /* Aspect ratio */
            m = MenuHelper.AddMenuItem("Aspect Ratio", "ASPECT", Keys.None, videoCount > 0, false, Resources.menuAspect.ToBitmap(), menuHandler);
            var currentAspect = Video.VideoControl.AspectRatio;
            foreach (var aspect in (AspectRatioMode[])Enum.GetValues(typeof(AspectRatioMode)))
            {
                m.DropDownItems.Add(MenuHelper.AddMenuItem(EnumUtils.GetDescriptionFromEnumValue(aspect),
                                                           string.Format("ASPECT-{0}", (int)aspect),
                                                           Keys.None, true, aspect == currentAspect, null,
                                                           menuHandler));
            }
            menu.DropDownItems.Add(m);
            /* Zoom */
            m = MenuHelper.AddMenuItem("Zoom", "Zoom", Keys.None, videoCount > 0, false, Resources.menuZoom.ToBitmap(), menuHandler);
            var currentZoom = Video.VideoControl.ZoomRatio;
            foreach (var zoom in (ZoomRatioMode[])Enum.GetValues(typeof(ZoomRatioMode)))
            {
                m.DropDownItems.Add(MenuHelper.AddMenuItem(EnumUtils.GetDescriptionFromEnumValue(zoom),
                                                           string.Format("ZOOM-{0}", (int)zoom),
                                                           Keys.None, true, zoom == currentZoom, null,
                                                           menuHandler));
            }
            menu.DropDownItems.AddRange(new ToolStripItem[]
                                            {
                                                m,
                                                new ToolStripSeparator(),
                                                MenuHelper.AddMenuItem("Effects", "EFFECTS",
                                                                       Keys.None, true, false,
                                                                       Resources.menuVideoEffects.ToBitmap(),
                                                                       menuHandler),
                                                MenuHelper.AddMenuItem("Snap shot", "SNAPSHOT",
                                                                       Keys.Control | Keys.C, true, false,
                                                                       Resources.menuSnap.ToBitmap(),
                                                                       menuHandler)
                                            });
            IsControlHovering = true;
        }
        #endregion

        #region Subtitles menu
        private static void BuildMenuSubtitles(ToolStripDropDownItem menu, EventHandler menuHandler, bool clear = true)
        {
            if (clear)
            {
                menu.DropDownItems.Clear();
            }
            var subtitlesCount = Video.VideoControl.SubtitlesCount;
            var current = Video.VideoControl.Subtitles;

            menu.DropDownItems.AddRange(new ToolStripItem[]
                                            {
                                                MenuHelper.AddMenuItem("Add Subtitle File", "ADD", Keys.None, true, false, Resources.menuSubFileAdd.ToBitmap(), menuHandler),
                                                new ToolStripSeparator()
                                            });
            var m = MenuHelper.AddMenuItem("Subtitle Track", "TRACKS", Keys.None, subtitlesCount > 0, false, Resources.menuSubFile.ToBitmap(), menuHandler);
            var subs = Video.VideoControl.SubtitleDescription;
            for (var t = 0; t <= subs.Count - 1; ++t)
            {
                var m1 = MenuHelper.AddMenuItem(subs[t].psz_name,
                    (t == 0 ? "DISABLE" : subs[t].i_id.ToString(CultureInfo.InvariantCulture)),
                    Keys.None, true, (t == 0 && current == -1 || subs[t].i_id == current), null, menuHandler);
                m.DropDownItems.Add(m1);
            }
            menu.DropDownItems.Add(m);
            IsControlHovering = true;
        }
        #endregion

        #region Preferences menu
        private static void BuildMenuPrefs(ToolStripDropDownItem menu, EventHandler menuHandler)
        {
            menu.DropDownItems.Clear();
            var skinIco = Resources.menuSkin.ToBitmap();
            var m = MenuHelper.AddMenuItem("Skin", "SKIN", Keys.None, true, false, skinIco, menuHandler);
            var i = 0;
            foreach (var skin in SkinManager.AvailableSkins)
            {
                m.DropDownItems.Add(MenuHelper.AddMenuItem(skin.Name, String.Format("SKIN-{0}", i), Keys.None, true,
                                                           AppPath.MainDir(SettingsManager.Settings.Window.CurrentSkin).ToLower() ==
                                                           skin.Path.ToLower(), null, menuHandler));
                i++;
            }
            /* Build rest of menu */
            menu.DropDownItems.AddRange(
                new ToolStripItem[]
                    {
                        m,
                        new ToolStripSeparator(),
                        MenuHelper.AddMenuItem("Effects", "EFFECTS", Keys.Control | Keys.E, true, false, Resources.menuEffects.ToBitmap(), menuHandler),
                        MenuHelper.AddMenuItem("Options...", "OPTIONS", Keys.Control | Keys.P, true, false, Resources.menuOptions.ToBitmap(), menuHandler)
                    });
            IsControlHovering = true;
        }
        #endregion

        #region Help menu
        private static void BuildMenuHelp(ToolStripDropDownItem menu, EventHandler menuHandler)
        {
            menu.DropDownItems.AddRange(
                new ToolStripItem[]
                    {
                        MenuHelper.AddMenuItem("About xsMedia...", "ABOUT", Keys.None, true, false, Resources.menuAbout.ToBitmap(),
                                               menuHandler),
                        new ToolStripSeparator(),
                        MenuHelper.AddMenuItem("VideoLAN Project", "VLC", Keys.None, true, false, Resources.menuVlc.ToBitmap(),
                                               menuHandler)
                    }
                );
            IsControlHovering = true;
        }
        #endregion
        #endregion

        #region Context menus
        #region Open menu
        private static void BuildOpenMenu(ToolStrip menu, EventHandler menuHandler, CdManager cdManager)
        {
            menu.Items.Clear();
            menu.Items.AddRange(OpenMenu(menuHandler, cdManager));
            IsControlHovering = true;
        }

        private static ToolStripItem[] OpenMenu(EventHandler menuHandler, CdManager cdManager)
        {
            var items = new List<ToolStripItem>();
            items.AddRange(
                new ToolStripItem[]
                {
                    MenuHelper.AddMenuItem("Open Media Files", "OPEN", Keys.Control | Keys.O, true,
                        false,
                        Resources.menuFile.ToBitmap(),
                        menuHandler),
                    MenuHelper.AddMenuItem("Open Media Folder", "FOLDER", Keys.None, true,
                        false,
                        Resources.menuFolder.ToBitmap(),
                        menuHandler),
                    MenuHelper.AddMenuItem("Open Network Stream", "NET", Keys.Control | Keys.N,
                        true, false,
                        Resources.menuNet.ToBitmap(),
                        menuHandler),
                    MenuHelper.AddMenuItem("Open Playlist", "LIST", Keys.Control | Keys.L, true,
                        false,
                        Resources.menuList.ToBitmap(),
                        menuHandler),
                    new ToolStripSeparator()
                });
            var m = MenuHelper.AddMenuItem("Open DiscData", Resources.menuDisc.ToBitmap());

            foreach (var drv in cdManager.AvailableDrives)
            {
                m.DropDownItems.Add(MenuHelper.AddMenuItem(drv.Value.DriveLabel + " " + drv.Value.VolumeLabel,
                                                           drv.Key.ToString(CultureInfo.InvariantCulture), Keys.None,
                                                           true, false, Resources.menuDiscDrive.ToBitmap(),
                                                           menuHandler));
            }
            items.AddRange(new ToolStripItem[] {m, new ToolStripSeparator()});
            /* Favorites */
            m = new ToolStripMenuItem("Favorites", Resources.menuFavoriteItem.ToBitmap());
            if (SettingsManager.Settings.Favorites.Favorite.Count > 0)
            {
                for (var i = 0; i <= 25; i++)
                {
                    if (i > SettingsManager.Settings.Favorites.Favorite.Count - 1)
                    {
                        break;
                    }
                    var f = SettingsManager.Settings.Favorites.Favorite[i];
                    m.DropDownItems.Add(MenuHelper.AddMenuItem(f.ToString(), f.FilePath, Keys.None, true, false,
                        Resources.menuFavoriteItem.ToBitmap(), OnFavoritesMenuItemClicked));
                }
                m.DropDownItems.Add(new ToolStripSeparator());
            }
            m.DropDownItems.Add(MenuHelper.AddMenuItem("Manage favorites", "MANAGE", Keys.Control | Keys.H, true,
                false, Resources.menuFavoriteEdit.ToBitmap(), OnFavoritesMenuItemClicked));
            items.Add(m);
            /* History */
            if (SettingsManager.Settings.Player.FileHistory.Count > 0)
            {
                m = new ToolStripMenuItem("Open recent", Resources.menuRecent.ToBitmap());
                foreach (var h in SettingsManager.Settings.Player.FileHistory)
                {
                    m.DropDownItems.Add(MenuHelper.AddMenuItem(h.ToString(), h.FilePath, Keys.None, true, false, Resources.menuRecentItem.ToBitmap(), OnRecentMenuItemClicked));
                }
                m.DropDownItems.AddRange(new ToolStripItem[]
                {
                    new ToolStripSeparator(),
                    MenuHelper.AddMenuItem("Clear history", "CLEAR", Keys.None, true, false, Resources.menuClear.ToBitmap(), OnRecentMenuItemClicked)
                });
                items.Add(m);
            }
            return items.ToArray();
        }
        #endregion

        #region Video window menu
        private static void BuildVideoWindowMenu()
        {
            MenuVideoWindow.Items.Clear();
            MenuVideoWindow.Items.AddRange(VideoWindowMenu());
        }

        private static ToolStripItem[] VideoWindowMenu()
        {
            /* Build context menu - Glenn 20 */
            var items = new List<ToolStripItem>();
            if (Video.VideoControl.IsVideo && Video.VideoControl.PlayerState != MediaState.Stopped)
            {
                if (!Video.IsFullScreen)
                {
                    items.Add(new ToolStripMenuItem("Full screen", Resources.menuFullScreen.ToBitmap(),
                        OnVideoWindowFullScreenMenuClick));
                }
                else
                {
                    items.Add(new ToolStripMenuItem("Exit full screen", Resources.menuFullScreenExit.ToBitmap(), OnVideoWindowFullScreenMenuClick));
                }
                if (Video.VideoControl.OpenDiscType == DiscType.None)
                {
                    items.Add(MenuHelper.AddMenuItem("Add to favorites", "ADD", Keys.None, true, false, Resources.menuFavoriteEdit.ToBitmap(), OnVideoMenuItemClicked));
                }
            }
            var m = new ToolStripMenuItem { Text = @"Playback speed", Image = Resources.menuPlaySpeed.ToBitmap() };
            BuildMenuPlaybackSpeed(m, OnPlaybackSpeedItemClicked);
            items.AddRange(new ToolStripItem[] { m, new ToolStripSeparator() });

            m = new ToolStripMenuItem { Text = @"Audio", Image = Resources.menuAudioTrack.ToBitmap() };
            BuildMenuAudio(m, OnAudioMenuItemClicked, false);
            items.Add(m);

            m = new ToolStripMenuItem { Text = @"Video", Image = Resources.menuVideoTrack.ToBitmap() };
            BuildMenuVideo(m, OnVideoMenuItemClicked, false);
            items.Add(m);

            m = new ToolStripMenuItem { Text = @"Subtitles", Image = Resources.menuSubFile.ToBitmap() };
            BuildMenuSubtitles(m, OnSubtitlesMenuItemClicked, false);
            items.Add(m);
            return items.ToArray();
        }
        #endregion

        #region Playlist window
        /* Window context menu */
        private static void BuildPlaylistMenu(ToolStrip menu, EventHandler menuHandler)
        {
            menu.Items.AddRange(PlaylistMenu(menuHandler));
        }

        private static ToolStripItem[] PlaylistMenu(EventHandler menuHandler)
        {
            var items = new List<ToolStripItem>
                            {
                                MenuHelper.AddMenuItem("New...", "NEW", Keys.None, true, false, Resources.menuNew.ToBitmap(), menuHandler),
                                new ToolStripSeparator(),
                                MenuHelper.AddMenuItem("Open Playlist", "OPEN", Keys.None, true, false, Resources.menuList.ToBitmap(),
                                                       menuHandler),
                                MenuHelper.AddMenuItem("Save Playlist", "SAVE", Keys.None, true, false, Resources.menuSave.ToBitmap(),
                                                       menuHandler),
                                new ToolStripSeparator(),
                                MenuHelper.AddMenuItem("Add Media File(s)", "ADDFILES", Keys.None, true, false, Resources.menuFile.ToBitmap(),
                                                       menuHandler),
                                MenuHelper.AddMenuItem("Add Media Folder", "ADDFOLDER", Keys.None, true, false, Resources.menuFolder.ToBitmap(),
                                                       menuHandler),
                                new ToolStripSeparator()
                            };

            var m = MenuHelper.AddMenuItem("Sort...", Resources.menuSort.ToBitmap());
            m.DropDownItems.Add(MenuHelper.AddMenuItem("By Title", "SORTTITLE", Keys.None, true, false, Resources.menuSort.ToBitmap(),
                                                       menuHandler));
            m.DropDownItems.Add(MenuHelper.AddMenuItem("By Artist", "SORTARTIST", Keys.None, true, false, Resources.menuSort.ToBitmap(),
                                                       menuHandler));
            m.DropDownItems.Add(MenuHelper.AddMenuItem("By File Location", "SORTLOCATION", Keys.None, true, false, Resources.menuSort.ToBitmap(),
                                                       menuHandler));
            m.DropDownItems.Add(MenuHelper.AddMenuItem("Randomize", "SORTRANDOM", Keys.None, true, false, Resources.menuSort.ToBitmap(),
                                                       menuHandler));

            items.Add(m);
            items.AddRange(
                new ToolStripItem[]
                    {
                        new ToolStripSeparator(),
                        MenuHelper.AddMenuItem("Remove Dead Files", "REMOVEDEAD", Keys.None, true, false, Resources.menuRemove.ToBitmap(), menuHandler),
                        MenuHelper.AddMenuItem("Remove Duplicates", "REMOVEDUP", Keys.None, true, false, Resources.menuRemove.ToBitmap(), menuHandler)
                    }
                );
            return items.ToArray();
        }

        /* File context menu */
        private static void BuildMenuPlaylistItem(ToolStrip menu, EventHandler menuHandler)
        {
            menu.Items.Clear();
            menu.Items.AddRange(PlaylistMenu(menuHandler));
            menu.Items.AddRange(
                new ToolStripItem[]
                    {
                        MenuHelper.AddMenuItem("Remove selected", "REMOVE", Keys.None, true, false, Resources.menuRemove.ToBitmap(), menuHandler),
                        new ToolStripSeparator(),
                        MenuHelper.AddMenuItem("Add to favorites", "ADD", Keys.None, Playlist.PlaylistControl.SelectedIndex != -1, false,
                                               Resources.menuFavoriteEdit.ToBitmap(), OnVideoMenuItemClicked),
                        MenuHelper.AddMenuItem("Media information", "INFO", Keys.None, Playlist.PlaylistControl.SelectedIndex != -1, false,
                                               Resources.menuInfo.ToBitmap(), menuHandler)
                    }
                );           
        }
        #endregion
        #endregion
        #endregion
    }
}
