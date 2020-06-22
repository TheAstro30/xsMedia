/* xsMedia - Media Player
 * (c)2013 - 2020
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Windows.Forms;
using xsCore.Utils.SystemUtils;
using xsMedia.Forms;

namespace xsMedia
{
    static class Program
    {
        /* Entry point */
        [STAThread]
        private static void Main(string[] args)
        {
            var command = args != null && args.Length > 0 ? args[0] : null;
            if (AppMessenger.CheckPrevInstance(command))
            {
                /* Do NOT run another instance and then do this check, or wrap this method in a try/catch for 2 reasons:
                 * 1). Attempting to exit the app on the second instance in the form's constructor before allowing it to initialize
                 * properly causes an ObjectDisposedException (crash in release mode); and
                 * 2). Wrapping this method in a try/catch hides that exception and ALL other exceptions (if the catch clause is
                 * set to "Application.Exit()"), so if an error occurs elsewhere, it gracefully exits and doesn't allow the debugger
                 * to show you WHERE the exception occured - bad programming practice */
                Application.Exit(); /* This command isn't really needed as this instance was never started with Application.Run() */
                return;
            }
            /* Allow program to initiate normally */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPlayer(command));
        }
    }
}
