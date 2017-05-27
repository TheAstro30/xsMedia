using System;
using System.Runtime.InteropServices;

namespace xsVlc.Core.Exceptions
{
    [Serializable]
    public class LibVlcException : Exception
    {
        public LibVlcException() : base(Marshal.PtrToStringAnsi(Interop.Api.libvlc_errmsg()))
        {
            /* Empty by default */
        }

        public LibVlcException(string message) : base(message)
        {
            /* Empty by default */
        }
    }
    
    [Serializable]
    public class LibVlcInitException : LibVlcException
    {
        private const string Msg = "Failed to initialize libVLC. Possible reasons : Some of the arguments may be incorrect. VLC dlls' version mismatch.";

        public LibVlcInitException() : base(Msg)
        {
            /* Empty by default */
        }
    }
    
    [Serializable]
    public class LibVlcNotFoundException : DllNotFoundException
    {
        private const string Msg = "Failed to load VLC modules. Make sure libvlc.dll, libvlccore.dll and plugins directory located in the executable path.";

        public LibVlcNotFoundException() : base(Msg)
        {
            /* Empty by default */
        }

        public LibVlcNotFoundException(Exception ex) : base(Msg, ex)
        {
            /* Empty by default */
        }
    }
}
