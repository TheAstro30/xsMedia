/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace xsCore.Utils
{
    /*  Recursive folder/file search class
     *  By Jason James Newland
     *  (C)Copyright 2010-2013
     *  KangaSoft Software - All Rights Reserved
     */
    public class FolderSearch
    {
        private const string DefaultFileMask = "*.*";
        private const string DefaultDirectoryMask = "*";

        private DirectoryInfo _initialDirectory;

        private List<string> _directoryMasks;
        private List<string> _fileMasks;

        private bool _recursive;

        public event Action<string> OnFileFound;
        public event Action<FolderSearch> OnFileSearchCompleted;

        /* Constructor */
        public FolderSearch()
        {
            _directoryMasks = new List<string>();
            _fileMasks = new List<string>();
        }

        /* Search methods */
        public void BeginSearch(DirectoryInfo initalDirectory, string fileMask, string directoryMask, bool recursive)
        {
            if (initalDirectory != null)
            {
                _initialDirectory = initalDirectory;
            }
            else
            {
                return;
            }
            _fileMasks = string.IsNullOrEmpty(fileMask) ? new List<string> { DefaultFileMask } : ParseMask(fileMask);
            _directoryMasks = string.IsNullOrEmpty(directoryMask) ? new List<string> { DefaultDirectoryMask } : ParseMask(directoryMask);
            _recursive = recursive;
            /* Start background thread */
            ThreadStart starter = () => ThreadSearch(_initialDirectory);
            var t = new Thread(starter)
                        {
                            IsBackground = true
                        };
            t.Start();
        }

        private void BeginSearch(DirectoryInfo baseDirectory)
        {
            try
            {
                foreach (var f in _fileMasks.Select(baseDirectory.GetFiles).SelectMany(files => files.Where(f => OnFileFound != null)))
                {
                    if (OnFileFound != null)
                    {
                        OnFileFound(f.FullName);
                    }
                }
            }
            catch (AccessViolationException)
            {
                /* Quietly handle exception */
            }
            try
            {
                if (!_recursive) { return; }
                var directories = new List<DirectoryInfo>();
                foreach (var dm in _directoryMasks)
                {
                    directories.AddRange(baseDirectory.GetDirectories(dm));
                }
                foreach (var di in directories)
                {
                    BeginSearch(di);
                }
            }
            catch (AccessViolationException)
            {
                /* Quietly handle exception */
            }
        }

        /* Private methods */
        private static List<string> ParseMask(string mask)
        {
            if (string.IsNullOrEmpty(mask))
            {
                return null;
            }
            mask = mask.Trim(';');
            if (mask.Length == 0)
            {
                return null;
            }
            var masks = new List<string>();
            masks.AddRange(mask.Split(';'));
            return masks;
        }

        private void ThreadSearch(DirectoryInfo directory)
        {
            BeginSearch(directory);
            if (OnFileSearchCompleted != null)
            {
                OnFileSearchCompleted(this);
            }
        }        
    }
}