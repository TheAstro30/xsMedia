/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace xsCore.Utils.UI
{
    public static class MenuHelper
    {
        public static ToolStripMenuItem AddMenuItem(string text)
        {
            return AddMenuItem(text, null, Keys.None, true, false, null, null);
        }

        public static ToolStripMenuItem AddMenuItem(string text, Image image)
        {
            return AddMenuItem(text, null, Keys.None, true, false, image, null);
        }

        public static ToolStripMenuItem AddMenuItem(string text, string key, EventHandler callBack)
        {
            return AddMenuItem(text, key, Keys.None, true, false, null, callBack);
        }

        public static ToolStripMenuItem AddMenuItem(string text, string key, Keys shortCutKeys, EventHandler callBack)
        {
            return AddMenuItem(text, key, shortCutKeys, true, false, null, callBack);
        }

        public static ToolStripMenuItem AddMenuItem(string text, string key, Keys shortCutKeys, bool enabled, EventHandler callBack)
        {
            return AddMenuItem(text, key, shortCutKeys, enabled, false, null, callBack);
        }

        public static ToolStripMenuItem AddMenuItem(string text, string key, Keys shortCutKeys, bool enabled, bool bChecked, Image image, EventHandler callBack)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            var m = new ToolStripMenuItem(text, image, callBack)
            {
                ShortcutKeys = shortCutKeys,
                Tag = key,
                Enabled = enabled,
                Checked = bChecked
            };
            return m;
        }
    }
}
