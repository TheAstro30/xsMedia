/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Serialization;
using xsCore.Utils;

namespace xsCore.Skin
{
    [Serializable, XmlRoot("skin")]
    public class InternalSkinFormat
    {
        public class HeaderInfo
        {
            [XmlAttribute("name")]
            public string Name;

            [XmlAttribute("author")]
            public string Author;

            [XmlAttribute("version")]
            public string Version;

            [XmlAttribute("copyright")]
            public string Copyright;

            [XmlAttribute("comment")]
            public string Comment;
        }

        public class SkinImage
        {
            public class SkinBaseImage
            {
                [XmlAttribute("src")]
                public string Path { get; set; }

                [XmlAttribute("id")]
                public string Id { get; set; }

                [XmlIgnore]
                public Bitmap PhysicalImage { get; set; }
            }

            [XmlElement("image")]
            public List<SkinBaseImage> Image = new List<SkinBaseImage>();
        }

        [XmlElement("head")]
        public HeaderInfo Header = new HeaderInfo();

        [XmlElement("images")]
        public SkinImage Images = new SkinImage();

        [XmlElement("body")]
        public SkinBody Body = new SkinBody();
    }

    [Serializable]
    public class SkinBody
    {
        public class ResourceBody
        {
            [XmlElement("resource")]
            public List<Resource> ResourceList = new List<Resource>();
        }

        [XmlElement("resources")]
        public ResourceBody Resources = new ResourceBody();

        [XmlElement("menu")]
        public MenuBody Renderer = new MenuBody();

        [XmlElement("playlist")]
        public MenuBody Playlist = new MenuBody();
    }

    /* Image data for current bitmap piece */
    [Serializable]
    public class ImageData
    {
        [XmlAttribute("imageId")]
        public string ImageId { get; set; }

        [XmlIgnore]
        public Rectangle ImageArea { get; set; }

        [XmlIgnore]
        public Size ImageSize { get; set; }

        [XmlIgnore]
        public Bitmap PhysicalImage { get; set; }

        [XmlAttribute("imageArea")]
        public string ImageAreaString
        {
            get
            {
                return ImageArea == Rectangle.Empty ? null : XmlFormatting.WriteRbRectangleFormat(ImageArea);
            }
            set
            {
                ImageArea = XmlFormatting.ParseRbRectangleFormat(value);
            }
        }

        [XmlAttribute("imageSize")]
        public string ImageSizeString
        {
            get
            {
                return ImageSize == Size.Empty ? null : XmlFormatting.WriteSizeFormat(ImageSize);
            }
            set
            {
                ImageSize = XmlFormatting.ParseSizeFormat(value);
            }
        }
    }

    /* Resource (control) data */
    [Serializable]
    public class Resource : ImageData
    {
        public class StateBody
        {
            public class StateData : ImageData
            {
                [XmlAttribute("id")]
                public string Id { get; set; }
            }

            [XmlElement("state")]
            public List<StateData> StateList = new List<StateData>();
        }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("area")]
        public String AreaString
        {
            get
            {
                return Area == Rectangle.Empty ? null : XmlFormatting.WriteRectangleFormat(Area);
            }
            set
            {
                Area = XmlFormatting.ParseRectangleFormat(value);
            }
        }

        [XmlAttribute("areaOffset")]
        public String AreaOffsetString
        {
            get
            {
                return AreaOffset == Size.Empty ? null : XmlFormatting.WriteSizeFormat(AreaOffset);
            }
            set
            {
                AreaOffset = XmlFormatting.ParseSizeFormat(value);
            }
        }

        [XmlElement("states")]
        public StateBody States = new StateBody();

        [XmlElement("rail")]
        public SliderRail Rail = new SliderRail();

        [XmlIgnore]
        public Rectangle Area { get; set; }

        [XmlIgnore]
        public Size AreaOffset { get; set; }
    }

    /* Slider rail attributes */
    [Serializable]
    public class SliderRail : ImageData
    {
        [XmlAttribute("area")]
        public string RailAreaString
        {
            get
            {
                return Area == Rectangle.Empty ? null : XmlFormatting.WriteRbRectangleFormat(Area);
            }
            set
            {
                Area = XmlFormatting.ParseRbRectangleFormat(value);
            }
        }

        [XmlAttribute("color")]
        public string RailColorString
        {
            get { return Color == Color.Empty ? null : ColorTranslator.ToHtml(Color); }
            set { Color = ColorTranslator.FromHtml(value); }
        }

        [XmlIgnore]
        public Color Color { get; set; }

        [XmlIgnore]
        public Rectangle Area { get; set; }
    }

    [Serializable]
    public class MenuBody
    {
        public class MenuRendererData
        {
            [XmlAttribute("id")]
            public string Id { get; set; }

            [XmlAttribute("color")]
            public string MenuColorString
            {
                get { return Color == Color.Empty ? null : ColorTranslator.ToHtml(Color); }
                set { Color = ColorTranslator.FromHtml(value); }
            }

            [XmlIgnore]
            public Color Color { get; set; }
        }

        [XmlElement("item")]
        public List<MenuRendererData> RendererData = new List<MenuRendererData>();
    }
}
