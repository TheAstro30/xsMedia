/* xsMedia - Media Player
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Linq;
using System.Windows.Forms;

namespace xsCore.Utils.SystemUtils
{
    public static class Monitor
    {
        /* Quick util for getting the current monitor the application is running on */
        public static Screen GetCurrentMonitor(Form wnd)
        {
            foreach (var s in Screen.AllScreens.Where(s => s.Bounds.Contains(wnd.Bounds)))
            {
                return s;
            }
            return Screen.PrimaryScreen;
        }
    }
}
