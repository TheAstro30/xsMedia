using System;
using System.Drawing;
using xsVlc.Common.Structures;

namespace xsVlc.Common
{
    public delegate void NewFrameEventHandler(Bitmap frame);
    public delegate void NewFrameDataEventHandler(PlanarFrame frame);
    public delegate void NewSoundEventHandler(Sound newSound);
    public delegate void VolumeChangedEventHandler(float volume, bool mute);

    public class AudioCallbacks
    {
        public VolumeChangedEventHandler VolumeCallback;
        public NewSoundEventHandler SoundCallback;
        public Action<long> PauseCallback;
        public Action<long> ResumeCallback;
        public Action<long> FlushCallback;
        public Action DrainCallback;
    }
}
