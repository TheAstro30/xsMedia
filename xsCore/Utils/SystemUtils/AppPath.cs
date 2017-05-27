using System;
using System.IO;
using System.Text;

namespace xsCore.Utils.SystemUtils
{
    public static class AppPath
    {
        /* These variables are used especially on closing out with DVD - AccessViolation otherwise */
        private static readonly string BaseFolder = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        /* Main application folder */
        public static string MainDir(string path)
        {
            /* Default is not to return application data roaming profile path */
            return MainDir(path, false);
        }

        public static string MainDir(string path, bool forceApplicationDataPath)
        {
            var folder = forceApplicationDataPath ? UserFolder : BaseFolder;
            if (!string.IsNullOrEmpty(path))
            {
                if (!forceApplicationDataPath && path.ToLower().Contains(folder.ToLower()))
                {
                    /* Remove app path */
                    return @"\" + path.Replace(folder, null);
                }
                var currentPath = path.Substring(0, 1) == @"\" ? (folder + path).Replace(@"\\", @"\") : path.Replace(@"\\", @"\");
                var pathOnly = Path.GetDirectoryName(currentPath);
                if (forceApplicationDataPath && !string.IsNullOrEmpty(pathOnly) && pathOnly.ToLower() != folder.ToLower() && !Directory.Exists(pathOnly))
                {
                    /* If the directory doesn't exists, create it */
                    Directory.CreateDirectory(pathOnly);
                }
                return currentPath;
            }
            /* Failed */
            return folder;
        }

        public static string TruncatePath(string path, int pathWidth)
        {
            var file = Path.GetFileName(path);
            var currentPath = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(currentPath))
            {
                return path;
            }
            /* Truncate current path */
            var finalPath = new StringBuilder();
            if (currentPath.Length > pathWidth)
            {
                finalPath.Append(string.Format("{0}...\\{1}", currentPath.Substring(0, pathWidth - 3), file));
                return finalPath.ToString();
            }
            return path;
        }
    }
}
