using System;

namespace xsVlc.Common.Structures
{
    [Serializable]
    public class SoundFormat
    {
        public SoundFormat(SoundType soundType, int rate, int channels)
        {
            SoundType = soundType;
            Format = soundType.ToString();
            Rate = rate;
            Channels = channels;
            Init();
            BlockSize = BitsPerSample / 8 * Channels;
            UseCustomAudioRendering = true;
        }

        private void Init()
        {
            switch (SoundType)
            {
                case SoundType.S16N:
                    BitsPerSample = 16;
                    break;
            }
        }

        public string Format { get; private set; }
        public int Rate { get; private set; }
        public int Channels { get; private set; }
        public int BitsPerSample { get; private set; }
        public SoundType SoundType { get; private set; }
        public int BlockSize { get; private set; }
        public bool UseCustomAudioRendering { get; set; }
    }
}
