/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace xsCore.Utils
{
    /*  Open/Save file dialog supported filters
     *  By Jason James Newland
     *  (C)Copyright 2011-2013
     *  KangaSoft Software - All Rights Reserved
     */
    public class FileMaskData
    {        
        /* Comparer - allows masks to be sorted in alpha-numeric order */
        public class CompareMasks : IComparer<FileMaskData>
        {
            public int Compare(FileMaskData x, FileMaskData y)
            {
                return x == null || y == null ? 0 : new CaseInsensitiveComparer().Compare(x.Mask, y.Mask);
            }
        }

        public FileMaskData()
        {
            /* Empty constructor */
        }

        public FileMaskData(string mask)
        {
            Mask = mask;
        }

        public string Mask { get; set; }
    }

    public sealed class FileMasks : IEnumerable<FileMaskData>
    {
        private readonly List<FileMaskData> _list = new List<FileMaskData>();

        /* Simple way to store the name and mask for file filters */
        public FileMasks(string description, string mask)
        {
            Description = description;
            _list.AddRange(mask.Split(';').Select(s => new FileMaskData(s)).ToList());
            _list.Sort(new FileMaskData.CompareMasks());
        }

        public string Description { get; set; }

        /* IEnumerable */
        public IEnumerator<FileMaskData> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /* Filters to string delimited by ; */
        public override string ToString()
        {
            return string.Join(";", _list.ToArray().Select(data => data.Mask).ToArray());
        }
    }

    public sealed class FilterMasks
    {
        /* Constructors */
        public FilterMasks()
        {
            FilterList = new List<FileMasks>();
        }        

        public FilterMasks(FilterMasks filterMasks, bool allowAllFiles, string allFiles)
        {            
            /* Copy/clone implementation */
            AllowAllFiles = allowAllFiles;
            AllFiles = allFiles;
            FilterList = new List<FileMasks>(filterMasks.FilterList);            
        }

        /* Properties */
        public bool AllowAllFiles { get; set; }
        public string AllFiles { get; set; }
        
        public List<FileMasks> FilterList;
        
        /* Adding methods */
        public void Add(FileMasks masks)
        {
            if (masks == null) { return; }            
            FilterList.Add(masks);
        }

        public void AddRange(FileMasks[] masks)
        {
            if (masks == null) { return; }
            FilterList.AddRange(masks);
        }

        public bool IsSupported(string file)
        {
            var ext = Path.GetExtension(file);
            return !string.IsNullOrEmpty(ext) &&
                   FilterList.Any(fm => fm.Any(data => data.Mask.ToUpper() == string.Format("*{0}", ext.ToUpper())));
        }

        /* Returns *.ext1;*.ext2 .. etc */
        public string SupportedMasks()
        {
            if (FilterList.Count == 0) { return string.Empty; }
            var sb = new StringBuilder();
            foreach (var fm in FilterList)
            {
                sb.AppendFormat("{0};", fm);
            }
            var final = sb.ToString();
            return final.EndsWith(";") ? final.Substring(0, final.Length - 1) : final;
        }

        /* ToString() override */        
        public override string ToString()
        {
            if (FilterList.Count == 0) { return string.Empty; }
            var sb = new StringBuilder();
            var other = new StringBuilder();
            /* Build a list of available filters */
            foreach (var fm in FilterList)
            {
                sb.AppendFormat("{0};", fm);
                other.AppendFormat("{0}{1}{2}{3}", (other.Length > 0 ? "|" : null), fm.Description, '|', fm);
            }
            if (AllowAllFiles)
            {
                var tmp = sb.ToString();
                return string.Format("{0}|{1}|{2}", AllFiles,
                                     tmp.EndsWith(";") ? tmp.Substring(0, tmp.Length - 1) : tmp, other);
            }
            return other.ToString();
        }     
    }
}
