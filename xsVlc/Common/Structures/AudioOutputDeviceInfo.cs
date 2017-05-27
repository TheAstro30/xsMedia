using System;

namespace xsVlc.Common.Structures
{
    [Serializable]
    public class AudioOutputDeviceInfo
    {
        public string Id;
        public string Longname;
    }

    [Serializable]
    public class AudioOutputModuleInfo
    {
        public string Name;
        public string Description;
    }
}
