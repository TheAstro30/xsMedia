/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.IO;

namespace xsCore.Utils.SystemUtils
{
    public static class FileInfoExtensions
    {
        public static FileInfo MakeUnique(this FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException("fileInfo");
            }
            var newFileName = new FileUtilities().GetNextFileName(fileInfo.FullName);
            if (newFileName == null)
            {
                throw new NullReferenceException("newFileName");
            }
            return new FileInfo(newFileName);
        }
    }

    public class FileUtilities
    {
        private const int MaxAttempts = 1024;

        public string GetNextFileName(string fullFileName)
        {
            if (string.IsNullOrEmpty(fullFileName))
            {
                throw new ArgumentNullException("fullFileName");
            }
            /* First check it doesn't already exist */
            if (!File.Exists(fullFileName))
            {
                return fullFileName;
            }
            var file = Path.GetFileName(fullFileName);
            var path = Path.GetDirectoryName(fullFileName);
            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(path))
            {
                return fullFileName;
            }
            var fileBase = Path.GetFileNameWithoutExtension(file);
            var ext = Path.GetExtension(file);
            /* Build hash set of filenames for performance */
            var files = new HashSet<string>(Directory.GetFiles(path));
            for (var index = 0; index < MaxAttempts; index++)
            {
                /* Try incrementally adding an index */
                var name = (index == 0)
                    ? file
                    : string.Format("{0} ({1}){2}", fileBase, index, ext);

                /* Check if exists */
                var fullPath = Path.Combine(path, name);
                if (files.Contains(fullPath))
                {
                    continue;
                }
                return fullPath;
            }
            /* Failed because exceeded maximum attempts */
            throw new Exception("Could not create unique filename in " + MaxAttempts + " attempts");
        }
    }
}
