/* xsMedia - Media Player
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using System.Windows.Forms;
using xsMedia.Forms;
using xsMedia.Logic;

namespace xsMedia
{
    /* This program has taken years (since 2013) of development, research and trial and error.
     * It originally started out using DirectX, which is why it was called xsMedia (extra small Media) Player.
     * This turned out to be way to much of a pain in the ass to work with, while VLC already existed; with built in codecs, so it
     * quickly turned in to a VLC project. I hope anyone reading this code appreciates the work that's gone in to this. I'm not the
     * best programmer in the world, and a LOT of the code could be re-written better. */
    internal static class Program
    {
        /* Entry point */
        [STAThread]
        private static void Main(string[] args)
        {
            /* !Single instance only! (Or, we'll have hundreds of windows open all trying to play at the same time) */
            var command = args != null && args.Length > 0 ? args[0] : null;
            var currentProcess = Process.GetCurrentProcess().ProcessName;
            using (var mutex = new Mutex(true, currentProcess))
            {
                if (mutex.WaitOne(0, false))
                {
                    /* Set this main instance as a "server" using Inter-Process Communication */
                    var channel = new IpcChannel(currentProcess);
                    ChannelServices.RegisterChannel(channel, false);
                    RemotingConfiguration.RegisterWellKnownServiceType(typeof (SingleInstance), "RemotingServer", WellKnownObjectMode.Singleton);
                    /* Allow program to initialize normally */
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FrmPlayer(command));
                }
                else
                {
                    /* We pass the command line to the "remote server" created above using Inter-Process Communication */
                    var channel = new IpcChannel();
                    ChannelServices.RegisterChannel(channel, false);
                    var instance = (SingleInstance)Activator.GetObject(typeof (SingleInstance), string.Format("ipc://{0}/RemotingServer", currentProcess));
                    instance.Execute(command);
                }
            }
        }

        private class SingleInstance : MarshalByRefObject
        {
            public void Execute(string args)
            {
                /* Execute the file to be played. I found if this is not sync executed, nothing happens (at all) - must be the calling thread to this is outside
                 * the UI thread; so this "nasty" work around seems to fix the problem */
                Player.Sync.Execute(() => Player.ProcessCommandLine(args));
            }
        }
    }
}