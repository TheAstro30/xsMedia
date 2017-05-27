using System;

namespace xsVlc.Common.Internal
{
    public interface ILogger
    {
        void Debug(string debug);
        void Info(string info);
        void Warning(string warn);
        void Error(string error);
    }

    public interface INativePointer
    {
        IntPtr Pointer { get; }
    }

    public interface IReferenceCount
    {
        void AddRef();
        void Release();
    }
}
