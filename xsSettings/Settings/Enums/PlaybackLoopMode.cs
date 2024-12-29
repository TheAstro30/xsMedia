/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.ComponentModel;

namespace xsSettings.Settings.Enums
{
    public enum PlaybackLoopMode
    {
        [Description("No loop")]
        None = 0,

        [Description("Loop one")]
        LoopOne = 1,

        [Description("Loop all")]
        LoopAll = 2,

        [Description("Shuffle play")]
        Shuffle = 3
    }
}
