/* xsMedia - xsPlaylist
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace xsPlaylist.Playlist.Playlists
{
    public class PlsPlaylist : IPlaylist
    {
        private List<PlaylistEntry> _list = new List<PlaylistEntry>();

        public PlsPlaylist()
        {
            /* Empty default constructor */
        }

        public PlsPlaylist(IPlaylist playlist)
        {
            if (playlist.Count == 0) { return; }
            /* Copy playlist */
            PlaylistTitle = playlist.PlaylistTitle;
            foreach (var entry in playlist)
            {
                Add(entry);
            }
        }

        /* Interface methods */
        public PlaylistType Type
        {
            get { return PlaylistType.Pls; }
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
                                if (!header && read.ToLower() == "[playlist]")
                                {
                                    header = true;
                                    continue;
                                }
                                if (!header || read.StartsWith("[")) { break; }
                                var sp = read.Split('=');
                                if (sp.Length < 2) { continue; }
                                /* Remove numeric values from item */
                                var item = Regex.Replace(sp[0], @"\d", "");
                                switch (item.ToUpper())
                                {
                                    case "FILE":
                                        if (pls != null) { _list.Add(pls); }
                                        pls = new PlaylistEntry
                                                  {
                                                      Location = sp[1]
                                                  };
                                        break;
                                    case "TITLE":
                                        if (pls != null)
                                        {
                                            var data = sp[1].Split('-');
                                            pls.Artist = data[0].Trim();
                                            pls.Title = data.Length > 1 ? data[1].Trim() : null;
                                        }
                                        break;
                                    case "LENGTH":
                                        if (pls != null)
                                        {
                                            int length;
                                            if (Int32.TryParse(sp[1], out length))
                                            {
                                                pls.Length = length > 0 ? length * 1000 : 0;
                                            }
                                        }
                                        break;
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
            /* Add what's left over */
            if (pls != null) { _list.Add(pls); }
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
                            sw.WriteLine("[playlist]");
                            for (var i = 1; i <= entries.Count; i++)
                            {
                                var pls = entries[i - 1];
                                sw.WriteLine("File{0}={1}", i, pls.Location);
                                if (!string.IsNullOrEmpty(pls.Artist) && !string.IsNullOrEmpty(pls.Title))
                                {
                                    sw.WriteLine("Title{0}={1} - {2}", i, pls.Artist, pls.Title);
                                }
                                sw.WriteLine("Length{0}={1}", i, pls.Length / 1000);
                            }
                            sw.WriteLine("NumberOfEntries={0}", _list.Count);
                            sw.WriteLine("Version=2");
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
