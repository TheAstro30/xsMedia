using System;
using System.Drawing;
using xsMedia.Properties;

namespace xsMedia.Helpers
{
    public sealed class MainIconUtil
    {
        public static Bitmap VideoWindowIcon()
        {
            /* Check system date - we want Christmas icon between the 13th and 26th Dec (12 days of Christmas, etc) */
            return DateSpan() ? Resources.videoWinIcon_xmas.ToBitmap() : Resources.videoWinIcon.ToBitmap();
        }

        private static bool DateSpan()
        {
            var dateToCheck = DateTime.Now;
            var startDate = new DateTime(DateTime.Now.Year, 12, 13);
            var endDate = new DateTime(DateTime.Now.Year, 12, 26);
            return dateToCheck >= startDate && dateToCheck < endDate;
        }
    }
}
