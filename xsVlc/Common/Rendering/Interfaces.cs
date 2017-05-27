using System;
using System.Drawing;
using xsVlc.Common.Structures;

namespace xsVlc.Common.Rendering
{
    public interface IRenderer : IDisposable
    {
        void SetExceptionHandler(Action<Exception> handler);
        int ActualFrameRate { get; }
    }

    public interface IAudioRenderer : IRenderer
    {
        void SetCallbacks(VolumeChangedEventHandler volume, NewSoundEventHandler sound);
        void SetCallbacks(AudioCallbacks callbacks);
        void SetFormat(SoundFormat format);
        void SetFormatCallback(Func<SoundFormat, SoundFormat> formatSetup);
    }

    public interface IMemoryRenderer : IRenderer
    {
        void SetCallback(NewFrameEventHandler callback);
        Bitmap CurrentFrame { get; }
        void SetFormat(BitmapFormat format);
    }

    public interface IMemoryRendererEx : IRenderer
    {
        void SetCallback(NewFrameDataEventHandler callback);
        PlanarFrame CurrentFrame { get; }
        void SetFormatSetupCallback(Func<BitmapFormat, BitmapFormat> setupCallback);
    }
}
