/* xsMedia - sxCore
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.Diagnostics;

namespace xsCore.Utils.SystemUtils
{
    public static class ExecuteProcess
    {
        public static bool BeginProcess(string process)
        {
            try
            {
                Process.Start(process);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
