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
