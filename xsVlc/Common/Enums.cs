using System.ComponentModel;

namespace xsVlc.Common
{
    public enum AspectRatioMode
    {
        [Description("Default")]
        Default = 0,

        [Description("1:1")]
        Mode1 = 1,

        [Description("4:3")]
        Mode2 = 2,

        [Description("16:9")]
        Mode3 = 3,

        [Description("16:10")]
        Mode4 = 4,

        [Description("2.21:1")]
        Mode5 = 5,

        [Description("2.35:1")]
        Mode6 = 6,

        [Description("2:39:1")]
        Mode7 = 8,

        [Description("5:4")]
        Mode8 = 9
    }

    public enum ZoomRatioMode
    {
        [Description("1:4 Quarter")]
        Mode1 = 0,

        [Description("1:2 Half")]
        Mode2 = 1,

        [Description("1:1 Original")]
        Mode3 = 2,

        [Description("2:1 Double")]
        Mode4 = 3
    }

    public enum AudioChannelType
    {
        Error = -1,
        Stereo = 1,
        RStereo = 2,
        Left = 3,
        Right = 4,
        Dolbys = 5
    }

    public enum AudioOutputDeviceType
    {
        AudioOutputDeviceError = -1,
        AudioOutputDeviceMono = 1,
        AudioOutputDeviceStereo = 2,
        AudioOutputDevice_2F2R = 4,
        AudioOutputDevice_3F2R = 5,
        AudioOutputDevice51 = 6,
        AudioOutputDevice61 = 7,
        AudioOutputDevice71 = 8,
        AudioOutputDeviceSpdif = 10
    }

    public enum StreamCategory
    {
        Unknown = 0,
        Audio = 1,
        Video = 2,
        Subtitle = 3,
        Data = 4
    }

    public enum ChromaType
    {
        Rv15 = 0,
        Rv16 = 1,
        Rv24 = 2,
        Rv32 = 3,
        Rgba = 4,
        Yv12 = 5,
        I420 = 6,
        Nv12 = 7,
        Yuy2 = 8,
        Uyvy = 9,
        J420 = 10
    }

    public enum DeinterlaceMode
    {
        [Description("Discard")]
        Discard = 0,

        [Description("Blend")]
        Blend = 1,

        [Description("Mean")]
        Mean = 2,

        [Description("Bob")]
        Bob = 3,

        [Description("Linear")]
        Linear = 4,

        [Description("X")]
        X = 5,

        [Description("Yadif")]
        Yadif = 6,

        [Description("Yadif 2X")]
        Yadif2X = 7
    }

    public enum MediaState
    {
        NothingSpecial = 0,
        Opening = 1,
        Buffering = 2,
        Playing = 3,
        Paused = 4,
        Stopped = 5,
        Ended = 6,
        Error = 7
    }

    public enum MetaDataType
    {
        Title,
        Artist,
        Genre,
        Copyright,
        Album,
        TrackNumber,
        Description,
        Rating,
        Date,
        Setting,
        Url,
        Language,
        NowPlaying,
        Publisher,
        EncodedBy,
        ArtworkUrl,
        TrackId
    }

    public enum NavigationMode
    {
        Activate = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    public enum PlaybackMode
    {
        Default = 0,
        Loop = 1,
        Repeat = 2
    }

    public enum Position
    {
        [Description("Center")]
        Center = 0,

        [Description("Left")]
        Left = 1,

        [Description("Right")]
        Right = 2,

        [Description("Top")]
        Top = 4,

        [Description("Bottom")]
        Bottom = 8,

        [Description("Top Right")]
        TopRight = Top | Right,

        [Description("Top Left")]
        TopLeft = Top | Left,

        [Description("Bottom Right")]
        BottomRight = Bottom | Right,

        [Description("Bottom Left")]
        BottomLeft = Bottom | Left
    }

    public enum SoundType
    {
        S16N = 0
    }

    public enum TrackType
    {
        Unknown = -1,
        Audio = 0,
        Video = 1,
        Text = 2,
    }

    public enum VideoCodecs
    {
        [Description("RV24")]
        Bgr24 = 0,

        [Description("RV32")]
        Bgr32 = 1,

        [Description("MJPG")]
        Mjpeg = 2,

        [Description("I420")]
        I420 = 3
    }

    public enum VlcColor : uint
    {
        [Description("Default")]
        Default = 0xf0000000,

        [Description("Black")]
        Black = 0x00000000,

        [Description("Gray")]
        Gray = 0x00808080,

        [Description("Silver")]
        Silver = 0x00C0C0C0,

        [Description("White")]
        White = 0x00FFFFFF,

        [Description("Maroon")]
        Maroon = 0x00800000,

        [Description("Red")]
        Red = 0x00FF0000,

        [Description("Fuchsia")]
        Fuchsia = 0x00FF00FF,

        [Description("Yellow")]
        Yellow = 0x00FFFF00,

        [Description("Olive")]
        Olive = 0x00808000,

        [Description("Green")]
        Green = 0x00008000,

        [Description("Teal")]
        Teal = 0x00008080,

        [Description("Lime")]
        Lime = 0x0000FF00,

        [Description("Purple")]
        Purple = 0x00800080,

        [Description("Navy Blue")]
        Navy = 0x00000080,

        [Description("Blue")]
        Blue = 0x000000FF,

        [Description("Aqua")]
        Aqua = 0x0000FFFF
    }
}
