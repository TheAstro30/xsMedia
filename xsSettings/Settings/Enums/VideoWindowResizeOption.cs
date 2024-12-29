/* xsMedia - xsSettings
 * (c)2013 - 2024
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System.ComponentModel;

namespace xsSettings.Settings.Enums
{
    public enum VideoWindowResizeOption
    {
        [Description("Player window to video size")]
        VideoSize = 0,

        [Description("Video to player window size")]
        WindowSize = 1
    }
}
