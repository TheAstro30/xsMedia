using System;

namespace xsVlc.Interop
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MinimalLibVlcVersion : Attribute
    {
        public MinimalLibVlcVersion(string minVersion)
        {
            MinimalVersion = minVersion;
        }

        public string MinimalVersion { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class MaxLibVlcVersion : Attribute
    {
        public MaxLibVlcVersion(string maxVersion)
        {
            MaxVersion = maxVersion;
        }

        public string MaxVersion { get; private set; }
    }
}
