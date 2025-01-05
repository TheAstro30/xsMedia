/* xsMedia - xsPlaylist
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace xsCore.Controls.Playlist.Playlist.Playlists
{
    public class M3UPlaylist : IPlaylist
    {
        private List<PlaylistEntry> _list = new List<PlaylistEntry>();
        private readonly PlaylistType _type;

        /* Constructors */
        public M3UPlaylist()
        {
            /* Empty default constructor */
            _type = PlaylistType.M3U;
        }

        public M3UPlaylist(PlaylistType type)
        {
            _type = type;
        }

        public M3UPlaylist(IPlaylist playlist)
        {
            if (playlist.Count == 0) { return; }
            /* Copy playlist */
            _type = playlist.Type;
            PlaylistTitle = playlist.PlaylistTitle;
            foreach (var entry in playlist)
            {
                Add(entry);
            }
        }

        /* Interface methods */
        public PlaylistType Type
        {
            get { return _type; }
        }

        public string PlaylistTitle { get; set; }

        public bool Read(string fileName)
        {
            bool success;
            var header = false;
            PlaylistEntry pls = null;
            _list = new List<PlaylistEntry>();
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                try
                {
                    using (var sr = new StreamReader(fs))
                    {
                        try
                        {
                            /* Parse line data */
                            while (!sr.EndOfStream)
                            {
                                var read = sr.ReadLine();
                                if (string.IsNullOrEmpty(read)) { continue; }
                                /* Header */
                                if (!header && read.ToLower() == "#extm3u")
                                {
                                    header = true;
                                    continue;
                                }
                                //if (!header) { break; }
                                if (read.ToLower().StartsWith("#extinf:"))
                                {
                                    var sp = read.Split(':');
                                    if (sp.Length == 1)
                                    {
                                        continue;
                                    }
                                    var data = sp[1].Split(',');
                                    int length;
                                    Int32.TryParse(data[0], out length);
                                    pls = new PlaylistEntry
                                              {
                                                  Length = length > 0 ? length * 1000 : 0
                                              };
                                    if (data.Length == 1)
                                    {
                                        continue;
                                    }
                                    var title = data[1].Split('-');
                                    pls.Artist = title[0].Trim();
                                    pls.Title = title.Length > 1 ? title[1].Trim() : null;
                                }
                                else
                                {
                                    if (pls == null)
                                    {
                                        pls = new PlaylistEntry();
                                    }
                                    pls.Location = read;
                                    _list.Add(pls);
                                    pls = null;
                                }
                            }
                            success = true;
                        }
                        catch { success = false; }
                        finally { sr.Close(); }
                    }
                }
                catch { success = false; }
                finally { fs.Close(); }
            }
            return success;
        }

        public bool Write(string fileName)
        {
            return Write(fileName, _list);
        }

        public bool Write(string fileName, IList<PlaylistEntry> entries)
        {
            if (entries.Count == 0) { return false; }
            bool success;
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                try
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        try
                        {
                            sw.WriteLine("#EXTM3U");
                            for (var i = 0; i <= entries.Count - 1; i++)
                            {
                                var pls = entries[i];
                                if (pls.Length != 0 || !string.IsNullOrEmpty(pls.Artist) || !string.IsNullOrEmpty(pls.Title))
                                {
                                    sw.WriteLine("#EXTINF:{0},{1} - {2}", pls.Length / 1000, pls.Artist, pls.Title);
                                }
                                sw.WriteLine(pls.Location);
                            }
                            sw.Flush();
                            success = true;
                        }
                        catch { success = false; }
                        finally { sw.Close(); }
                    }
                }
                catch { success = false; }
                finally { fs.Close(); }
            }
            return success;
        }

        public void Sort(PlaylistSortType sortType)
        {
            switch (sortType)
            {
                case PlaylistSortType.Title:
                    _list.Sort(new PlaylistEntry.CompareByTitle());
                    break;
                case PlaylistSortType.Artist:
                    _list.Sort(new PlaylistEntry.CompareByArtist());
                    break;
                case PlaylistSortType.Path:
                    _list.Sort(new PlaylistEntry.CompareByPath());
                    break;
            }
        }

        public void RemoveDuplicates()
        {
            _list = _list.Distinct(new PlaylistEntry.PlaylistDuplicateComparer()).ToList();
        }

        /* IList */
        public int IndexOf(PlaylistEntry item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, PlaylistEntry item)
        {
            if (index < 0 || index > _list.Count - 1)
            {
                return;
            }
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > _list.Count - 1)
            {
                return;
            }
            _list.RemoveAt(index);
        }

        public PlaylistEntry this[int index]
        {
            get { return index < 0 || index > _list.Count - 1 ? null : _list[index]; }
            set
            {
                if (index < 0 || index > _list.Count - 1)
                {
                    return;
                }
                _list[index] = value;
            }
        }

        public void Add(PlaylistEntry item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(PlaylistEntry item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(PlaylistEntry[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(PlaylistEntry item)
        {
            return _list.Remove(item);
        }

        /* IEnumerable */
        public IEnumerator<PlaylistEntry> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
