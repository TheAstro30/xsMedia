using System;

namespace xsVlc.Common.Vlm
{
    [Serializable]
    public class VlmEvent : EventArgs
    {
        public VlmEvent(string instanceName, string mediaName)
        {
            InstanceName = instanceName;
            MediaName = mediaName;
        }

        public string MediaName { get; private set; }
        public string InstanceName { get; private set; }
    }
}
