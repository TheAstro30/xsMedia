using System.Drawing;

namespace xsVlc.Common.Filters
{
    public interface IAdjustFilter
    {       
        bool Enabled { get; set; }         
        float Contrast { get; set; }    
        float Brightness { get; set; }
        int Hue { get; set; } 
        float Saturation { get; set; }
        float Gamma { get; set; }
    }

    public interface ICropFilter
    {
        bool Enabled { get; set; }
        Rectangle CropArea { get; set; }
    }

    public interface IDeinterlaceFilter
    {
        bool Enabled { get; set; }
        DeinterlaceMode Mode { get; set; }
    }

    public interface ILogoFilter
    {
        bool Enabled { get; set; }
        string File { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Delay { get; set; }
        int Repeat { get; set; }
        int Opacity { get; set; }
        Position Position { get; set; }
    }

    public interface IMarqueeFilter
    {
        bool Enabled { get; set; }

        /* Marquee text to display.
         * (Available format strings:
         * Time related: %Y = year, %m = month, %d = day, %H = hour,
         * %M = minute, %S = second, ... 
         * Meta data related: $a = artist, $b = album, $c = copyright,
         * $d = description, $e = encoded by, $g = genre,
         * $l = language, $n = track num, $p = now playing,
         * $r = rating, $s = subtitles language, $t = title,
         * $u = url, $A = date,
         * $B = audio bitrate (in kb/s), $C = chapter,
         * $D = duration, $F = full name with path, $I = title,
         * $L = time left,
         * $N = name, $O = audio language, $P = position (in %), $R = rate,
         * $S = audio sample rate (in kHz),
         * $T = time, $U = publisher, $V = volume, $_ = new line) */
        string Text { get; set; }

        VlcColor Color { get; set; }
        Position Position { get; set; }
        int Refresh { get; set; }
        int Size { get; set; }
        int Timeout { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int Opacity { get; set; }
    }
}