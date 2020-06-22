/* xsMedia - Media Player
 * (c)2013 - 2020
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
using xsCore.CdUtils;
using xsCore.Skin;
using xsCore.Utils;
using xsCore.Utils.SystemUtils;
using xsCore.Utils.UI;
using xsMedia.Forms;
using xsMedia.Properties;
using xsPlaylist;
using xsPlaylist.Forms;
using xsPlaylist.Playlist;
using xsPlaylist.Utils;
using xsSettings;
using xsSettings.Forms;
using xsVlc.Common;

namespace xsMedia.Logic
{
    /* Static class for menu construction (keeps code minimal on form) */
    public sealed class Menus
    {
        private static FrmPlayer _player;
        private static FolderSearch _skinsSearch;

        public static MenuStrip MainMenu { get; set; }
        public static ToolStripMenuItem MenuMedia { get; set; }
        public static ToolStripMenuItem MenuPrefs { get; set; }
        public static ToolStripMenuItem MenuAudio { get; set; }
        public static ToolStripMenuItem MenuVideo { get; set; }
        public static ToolStripMenuItem MenuSubtitles { get; set; }
        public static ToolStripMenuItem MenuHelp { get; set; }

        public static ToolStripMenuItem MenuFull { get; set; }
        public static ToolStripMenuItem MenuList { get; set; }
        
        public static ContextMenuStrip MenuOpen { get; set; }
        public static ContextMenuStrip MenuPlaylist { get; set; }
        public static ContextMenuStrip MenuPlaylistItem { get; set; }

        public static void Init(FrmPlayer player)
        {
            _player = player;
            MainMenu = new MenuStrip
                           {
                               Font = _player.Font,
                               ShowItemToolTips = true,
                               Dock = DockStyle.Top
                           };

            MenuMedia = new ToolStripMenuItem
                            {
                                Text = @"&Media",
                                Tag = "ROOT"
                            };
            MenuMedia.DropDownOpening += OnOpenMenuOpening;

            MenuPrefs = new ToolStripMenuItem
                            {
                                Text = @"&Preferences",
                                Tag = "ROOT"
                            };
            MenuPrefs.DropDownOpening += OnPrefsMenuOpening;

            MenuAudio = new ToolStripMenuItem
                            {
                                Text = @"&Audio",
                                Tag = "ROOT"
                            };
            MenuAudio.DropDownOpening += OnAudioMenuOpening;

            MenuVideo = new ToolStripMenuItem
                            {
                                Text = @"&Video",
                                Tag = "ROOT"
                            };
            MenuVideo.DropDownOpening += OnVideoMenuOpening;

            MenuSubtitles = new ToolStripMenuItem
                                {
                                    Text = @"&Subtitles",
                                    Tag = "ROOT"
                                };
            MenuSubtitles.DropDownOpening += OnSubtitlesMenuOpening;

            MenuHelp = new ToolStripMenuItem
                           {
                               Text = @"&Help",
                               Tag = "ROOT"
                           };

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

            MainMenu.Items.AddRange(new ToolStripItem[]
                                         {
                                             MenuMedia, MenuPrefs, MenuAudio, MenuVideo, MenuSubtitles, MenuHelp, MenuList, MenuFull
                                         });
            _player.Controls.Add(MainMenu);

            /* Context menus */
            MenuOpen = new ContextMenuStrip();
            MenuOpen.Opening += OnOpenMenuOpening;

            MenuPlaylist = new ContextMenuStrip();
            BuildPlaylistMenu(MenuPlaylist, OnPlaylistMenuItemClicked);

            MenuPlaylistItem = new ContextMenuStrip();
            BuildMenuPlaylistItem(MenuPlaylistItem, OnPlaylistMenuItemClicked);
            MenuPlaylistItem.Opening += OnPlaylistItemMenuOpening;
            
            BuildOpenMenu(MenuOpen, OnOpenMenuItemClicked, Video.VideoControl.CdControl);
            BuildMenuMedia(MenuMedia, OnOpenMenuItemClicked, Video.VideoControl.CdControl);
            BuildMenuVideo(MenuVideo, OnVideoMenuItemClicked);
            BuildMenuHelp(MenuHelp, OnHelpMenuItemClicked);

            /* Search for available skins */
            _skinsSearch = new FolderSearch();
            _skinsSearch.BeginSearch(new DirectoryInfo(AppPath.MainDir(@"\skins", false)), "*.xml", "*", true);
            _skinsSearch.OnFileFound += OnSkinFileFound;
            _skinsSearch.OnFileSearchCompleted += OnSkinFileComplete;
        }

        /* Callbacks/events - Playlist menu click */        
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

        /* Open menu */
        private static void OnOpenMenuOpening(object sender, EventArgs e)
        {
            BuildOpenMenu(MenuOpen, OnOpenMenuItemClicked, Video.VideoControl.CdControl);
            BuildMenuMedia(MenuMedia, OnOpenMenuItemClicked, Video.VideoControl.CdControl);
        }

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

        /* Prefs menu */
        private static void OnPrefsMenuOpening(object sender, EventArgs e)
        {
            BuildMenuPrefs(MenuPrefs, OnPrefsMenuItemClicked);
        }

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
                    using (var settings = new FrmSettings())
                    {
                        settings.ShowDialog(_player);
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

        /* Audio menu */
        private static void OnAudioMenuOpening(object sender, EventArgs e)
        {
            BuildMenuAudio(MenuAudio, OnAudioMenuItemClicked);
        }

        private static void OnAudioMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            var tag = o.Tag.ToString();
            switch (tag)
            {
                case "DISABLE":
                    Video.VideoControl.AudioTrack = -1;
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

        /* Video menu */
        private static void OnVideoMenuOpening(object sender, EventArgs e)
        {
            BuildMenuVideo(MenuVideo, OnVideoMenuItemClicked);
        }

        private static void OnVideoMenuItemClicked(object sender, EventArgs e)
        {
            var o = (ToolStripMenuItem)sender;
            var tag = o.Tag.ToString();
            switch (tag)
            {
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

        /* Subtitles menu */
        private static void OnSubtitlesMenuOpening(object sender, EventArgs e)
        {
            BuildMenuSubtitles(MenuSubtitles, OnSubtitlesMenuItemClicked);
        }

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
                                             Filter = Filters.SubtitleFilters.ToString()
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

        /* Help menu */
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

        /* Playlist menu */
        private static void OnPlaylistItemMenuOpening(object sender, EventArgs e)
        {
            BuildMenuPlaylistItem(MenuPlaylistItem, OnPlaylistMenuItemClicked);
        }

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

        /* Open menu */
        private static void BuildOpenMenu(ToolStrip menu, EventHandler menuHandler, CdManager cdManager)
        {
            menu.Items.Clear();
            menu.Items.AddRange(OpenMenu(menuHandler, cdManager));
        }

        /* Menubar - media menu */
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
        }

        /* Menubar - audio menu */
        private static void BuildMenuAudio(ToolStripDropDownItem menu, EventHandler menuHandler)
        {
            menu.DropDownItems.Clear();
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
            menu.DropDownItems.AddRange(new ToolStripItem[]
                                            {
                                                new ToolStripSeparator(),
                                                MenuHelper.AddMenuItem("Effects", "EFFECTS",
                                                                       Keys.None, true, false,
                                                                       Resources.menuEffects.ToBitmap(),
                                                                       menuHandler)
                                            });
        }

        /* Menubar - video menu */
        private static void BuildMenuVideo(ToolStripDropDownItem menu, EventHandler menuHandler)
        {
            menu.DropDownItems.Clear();
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
                                                                       Resources.menuEffects.ToBitmap(),
                                                                       menuHandler),
                                                MenuHelper.AddMenuItem("Snap shot", "SNAPSHOT",
                                                                       Keys.Control | Keys.C, true, false,
                                                                       Resources.menuSnap.ToBitmap(),
                                                                       menuHandler)
                                            });
        }

        /* Menubar - subtitles menu */
        private static void BuildMenuSubtitles(ToolStripDropDownItem menu, EventHandler menuHandler)
        {
            menu.DropDownItems.Clear();
            var subtitlesCount = Video.VideoControl.SubtitlesCount;
            var current = Video.VideoControl.Subtitles;
            var sub = Resources.menuSubFile.ToBitmap();
            menu.DropDownItems.AddRange(new ToolStripItem[]
                                            {
                                                MenuHelper.AddMenuItem("Add Subtitle File", "ADD", Keys.None, true, false, sub, menuHandler),
                                                new ToolStripSeparator()
                                            });            
            var m = MenuHelper.AddMenuItem("Subtitle Track", "TRACKS", Keys.None, subtitlesCount > 0, false, sub, menuHandler);
            var subs = Video.VideoControl.SubtitleDescription;
            for (var t = 0; t <= subs.Count - 1; ++t)
            {
                var m1 = MenuHelper.AddMenuItem(subs[t].psz_name,
                                                (t == 0 ? "DISABLE" : subs[t].i_id.ToString(CultureInfo.InvariantCulture)),
                                                Keys.None, true, (t == 0 && current == -1 || subs[t].i_id == current), null,
                                                menuHandler);
                m.DropDownItems.Add(m1);                
            }
            menu.DropDownItems.Add(m);
        }

        /* Menubar - preferences menu */
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
        }

        /* Menubar - help menu */
        private static void BuildMenuHelp(ToolStripDropDownItem menu, EventHandler menuHandler)
        {
            menu.DropDownItems.AddRange(
                new ToolStripItem[]
                    {
                        MenuHelper.AddMenuItem("About xsMedia...", "ABOUT", Keys.None, true, false, null,
                                               menuHandler),
                        new ToolStripSeparator(),
                        MenuHelper.AddMenuItem("VideoLAN Project", "VLC", Keys.None, true, false, null,
                                               menuHandler)
                    }
                );
        }

        /* Context menus */
        private static void BuildPlaylistMenu(ToolStrip menu, EventHandler menuHandler)
        {
            menu.Items.AddRange(PlaylistMenu(menuHandler));
        }

        private static void BuildMenuPlaylistItem(ToolStrip menu, EventHandler menuHandler)
        {
            menu.Items.Clear();
            menu.Items.AddRange(PlaylistMenu(menuHandler));
            menu.Items.AddRange(
                new ToolStripItem[]
                    {
                        MenuHelper.AddMenuItem("Remove selected", "REMOVE", Keys.None, true, false, Resources.menuRemove.ToBitmap(), menuHandler),
                        new ToolStripSeparator(),
                        MenuHelper.AddMenuItem("Media information", "INFO", Keys.None, Playlist.PlaylistControl.SelectedIndex != -1, false,
                                               Resources.menuInfo.ToBitmap(), menuHandler)
                    }
                );
        }

        private static ToolStripItem[] OpenMenu(EventHandler menuHandler, CdManager cdManager)
        {
            var items = new List<ToolStripItem>
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
                            };
            var m = MenuHelper.AddMenuItem("Open Disc", Resources.menuDisc.ToBitmap());

            foreach (var drv in cdManager.AvailableDrives)
            {
                m.DropDownItems.Add(MenuHelper.AddMenuItem(drv.Value.DriveLabel + " " + drv.Value.VolumeLabel,
                                                           drv.Key.ToString(CultureInfo.InvariantCulture), Keys.None,
                                                           true, false, Resources.menuDisc.ToBitmap(),
                                                           menuHandler));
            }
            items.Add(m);
            return items.ToArray();
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
    }
}
