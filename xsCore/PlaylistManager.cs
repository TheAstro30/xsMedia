/* xsMedia - xsPlaylist
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using xsCore.Controls.Playlist.Playlist;
using xsCore.Controls.Playlist.Playlist.Playlists;
using xsVlc.Common.Media;
using xsVlc.Core;

namespace xsCore
{
    public static class PlaylistManager
    {
        public static IPlaylist Playlist { get; set; }

        public static event Action<bool> PlaylistEntriesChanged;
        public static event Action<PlaylistEntry> PlaylistItemAdded;
        public static event Action<IList<int>> PlaylistItemRemoved;
        public static event Action<int, PlaylistEntry> PlaylistMetaDataChanged;
        public static event Action PlaylistSaved;

        /* Constructor */
        static PlaylistManager()
        {
            /* Default playlist type */
            Playlist = new XsplPlaylist();
            MediaList = new MediaPlayerFactory().CreateMediaList<IMediaList>();
        }

        public static bool ContentsChanged { get; set; }
        public static IMediaList MediaList { get; set; }

        /* New list */
        public static void New()
        {
            Clear(false);
        }

        /* Add media objects */
        public static void Add(IMedia media)
        {
            Add(media, null);
        }

        public static void Add(IMedia media, int length)
        {
            Add(media, null, length);
        }

        public static void Add(IMedia media, string[] options)
        {
            Add(media, options, 0);
        }

        public static void Add(IMedia media, string[] options, int length)
        {
            Add(media, options, length, null);
        }

        public static void Add(IMedia media, string[] options, int length, string title)
        {
            var entry = new PlaylistEntry
                            {
                                Location = media.Input,
                                Title = title,
                                Length = length,
                                Options = options != null ? options.ToList() : new List<string>()
                            };
            Playlist.Add(entry);
            MediaList.Add(media);
            ContentsChanged = true;
            if (PlaylistItemAdded != null)
            {
                PlaylistItemAdded(entry);
            }
        }

        /* Clear lists */
        public static void Clear()
        {
            Clear(true);            
        }

        public static void Sort(PlaylistSortType sortType)
        {
            var t = new Thread(() => BackgroundSort(sortType))
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        public static void Remove(IList<int> entries)
        {
            var t = new Thread(() => BackgroundRemoval(entries))
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        public static void RemoveDuplicates()
        {
            var t = new Thread(BackgroundDuplicateRemoval)
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        public static void RemoveDead()
        {
            var t = new Thread(BackgroundDeadRemoval)
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        /* Load/save methods */
        public static void Load(string fileName)
        {
            var t = new Thread(() => BackgroundLoad(fileName))
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        public static void Save(string fileName)
        {
            var t = new Thread(() => BackgroundSave(fileName))
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        /* Meta data */
        public static PlaylistEntry GetMetaData(int index)
        {
            return Playlist.Count == 0 || index > Playlist.Count - 1 ? null : Playlist[index];
        }

        public static void SetMetaData(int index, PlaylistEntry entry)
        {
            if (Playlist.Count == 0 || index > Playlist.Count - 1)
            {
                return;
            }
            Playlist[index] = entry;
            /* Pass it back out */
            if (PlaylistMetaDataChanged != null)
            {
                PlaylistMetaDataChanged(index, entry);
            }
        }

        /* Private methods */
        private static void Clear(bool changed)
        {
            Playlist.Clear();
            MediaList.Clear();
            ContentsChanged = changed;
            if (PlaylistEntriesChanged != null)
            {
                PlaylistEntriesChanged(false);
            }
        }

        private static void BackgroundLoad(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return; }
            switch (GetPlaylistType(fileName))
            {
                case PlaylistType.Xspf:
                    Playlist = new XspfPlaylist();
                    break;

                case PlaylistType.Wpl:
                    Playlist = new WplPlaylist();
                    break;

                case PlaylistType.M3U:
                    Playlist = new M3UPlaylist(PlaylistType.M3U);
                    break;

                case PlaylistType.M3U8:
                    Playlist = new M3UPlaylist(PlaylistType.M3U8);
                    break;

                case PlaylistType.Pls:
                    Playlist = new PlsPlaylist();
                    break;

                default:
                    Playlist = new XsplPlaylist();
                    break;
            }
            if (!Playlist.Read(fileName))
            {
                Playlist.Clear();
                MediaList.Clear();
                if (PlaylistEntriesChanged != null)
                {
                    PlaylistEntriesChanged(false);
                }
                return;
            }
            UpdateMediaObjects();
            ContentsChanged = false;
            if (PlaylistEntriesChanged != null)
            {
                PlaylistEntriesChanged(true);
            }
        }

        private static void BackgroundSave(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return; }
            var outputType = GetPlaylistType(fileName);
            bool success;
            if (outputType == Playlist.Type)
            {
                /* Just write out current list if the filename type is of current list type */
                success = Playlist.Write(fileName);
            }
            else
            {
                IPlaylist list;
                /* If the output type isn't the same as the input type, it will be converted */
                switch (outputType)
                {
                    case PlaylistType.Xspf:
                        list = new XspfPlaylist();
                        break;
                    case PlaylistType.Wpl:
                        list = new WplPlaylist();
                        break;
                    case PlaylistType.M3U:
                        list = new M3UPlaylist(PlaylistType.M3U);
                        break;
                    case PlaylistType.M3U8:
                        list = new M3UPlaylist(PlaylistType.M3U8);
                        break;
                    case PlaylistType.Pls:
                        list = new PlsPlaylist();
                        break;
                    default:
                        list = new XsplPlaylist();
                        break;
                }
                /* Write out playlist format passing current playlist entries */
                success = list.Write(fileName, Playlist);
            }
            if (success && PlaylistSaved != null)
            {
                PlaylistSaved();
            }
        }

        private static void BackgroundSort(PlaylistSortType sortType)
        {
            if (Playlist.Count == 0) { return; }
            switch (sortType)
            {
                case PlaylistSortType.Random:
                    /* Randomization */
                    var rnd = new Random();
                    IPlaylist listRnd = new XsplPlaylist();
                    while (Playlist.Any())
                    {
                        var i = rnd.Next(0, Playlist.Count);
                        listRnd.Add(Playlist[i]);
                        Playlist.RemoveAt(i);
                    }
                    Playlist = new XsplPlaylist(listRnd);
                    break;
                default:
                    Playlist.Sort(sortType);
                    break;
            }
            UpdateMediaObjects();
            if (PlaylistEntriesChanged != null)
            {
                PlaylistEntriesChanged(false);
            }
            ContentsChanged = true;
        }

        private static void BackgroundRemoval(IList<int> entries)
        {
            if (Playlist.Count == 0 || entries.Count == 0)
            {
                return;
            }
            for (var index = entries.Count - 1; index >= 0; --index)
            {
                Playlist.RemoveAt(entries[index]);
                MediaList.RemoveAt(entries[index]);
            }
            if (PlaylistItemRemoved != null)
            {
                PlaylistItemRemoved(entries);
            }
            ContentsChanged = true;
        }

        private static void BackgroundDuplicateRemoval()
        {
            if (Playlist.Count == 0) { return; }
            Playlist.RemoveDuplicates();
            UpdateMediaObjects();
            if (PlaylistEntriesChanged != null)
            {
                PlaylistEntriesChanged(false);
            }
            ContentsChanged = true;
        }

        private static void BackgroundDeadRemoval()
        {
            if (Playlist.Count == 0) { return; }
            var list = new List<int>();
            for (var index = Playlist.Count - 1; index >= 0; --index)
            {
                if (File.Exists(Playlist[index].Location)) { continue; }
                Playlist.RemoveAt(index);
                MediaList.RemoveAt(index);
                list.Add(index);
            }
            if (PlaylistItemRemoved != null)
            {
                PlaylistItemRemoved(list);
            }
            ContentsChanged = true;
        }

        private static void UpdateMediaObjects()
        {
            MediaList.Clear();
            /* Update media objects */
            foreach (var media in Playlist.Select(entry => new MediaPlayerFactory().CreateMedia<IMedia>(entry.Location, entry.Options.ToArray())))
            {
                MediaList.Add(media);
            }
        }

        private static PlaylistType GetPlaylistType(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(ext)) { return PlaylistType.Default; }
            switch (ext.ToUpper())
            {
                case ".XSPF":
                    return PlaylistType.Xspf;
                case ".WPL":
                    return PlaylistType.Wpl;
                case ".M3U":
                    return PlaylistType.M3U;
                case ".M3U8":
                    return PlaylistType.M3U8;
                case ".PLS":
                    return PlaylistType.Pls;
            }
            return PlaylistType.Default;
        }
    }
}
