using System;
using System.Drawing.Imaging;

namespace xsVlc.Common.Structures
{
    [Serializable]
    public class BitmapFormat
    {
        public BitmapFormat(int width, int height, ChromaType chroma)
        {
            Width = width;
            Height = height;
            ChromaType = chroma;
            Planes = 1;
            PlaneSizes = new int[3];

            Init();

            Chroma = ChromaType.ToString();
            if (IsRgb)
            {
                Pitch = Width * BitsPerPixel / 8;
                PlaneSizes[0] = ImageSize = Pitch * Height;
                Pitches = new[] { Pitch };
                Lines = new[] { Height };
            }
        }

        private void Init()
        {
            switch (ChromaType)
            {
                case ChromaType.Rv15:
                    PixelFormat = PixelFormat.Format16bppRgb555;
                    BitsPerPixel = 16;
                    break;

                case ChromaType.Rv16:
                    PixelFormat = PixelFormat.Format16bppRgb565;
                    BitsPerPixel = 16;
                    break;

                case ChromaType.Rv24:
                    PixelFormat = PixelFormat.Format24bppRgb;
                    BitsPerPixel = 24;
                    break;

                case ChromaType.Rv32:
                    PixelFormat = PixelFormat.Format32bppRgb;
                    BitsPerPixel = 32;
                    break;

                case ChromaType.Rgba:
                    PixelFormat = PixelFormat.Format32bppArgb;
                    BitsPerPixel = 32;
                    break;

                case ChromaType.Nv12:
                    BitsPerPixel = 12;
                    Planes = 2;
                    PlaneSizes[0] = Width * Height;
                    PlaneSizes[1] = Width * Height / 2;
                    Pitches = new[] { Width, Width };
                    Lines = new[] { Height, Height / 2 };
                    ImageSize = PlaneSizes[0] + PlaneSizes[1];
                    break;

                case ChromaType.I420:
                case ChromaType.Yv12:
                case ChromaType.J420:
                    BitsPerPixel = 12;
                    Planes = 3;
                    PlaneSizes[0] = Width * Height;
                    PlaneSizes[1] = PlaneSizes[2] = Width * Height / 4;
                    Pitches = new[] { Width, Width / 2, Width / 2 };
                    Lines = new[] { Height, Height / 2, Height / 2 };
                    ImageSize = PlaneSizes[0] + PlaneSizes[1] + PlaneSizes[2];
                    break;

                case ChromaType.Yuy2:
                case ChromaType.Uyvy:
                    BitsPerPixel = 16;
                    PlaneSizes[0] = Width * Height * 2;
                    Pitches = new[] { Width * 2 };
                    Lines = new[] { Height };
                    ImageSize = PlaneSizes[0];
                    break;

                default:
                    throw new ArgumentException("Unsupported chroma type " + ChromaType);
            }
        }

        public int Pitch { get; private set; }
        public int ImageSize { get; private set; }
        public string Chroma { get; private set; }
        public PixelFormat PixelFormat { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int BitsPerPixel { get; private set; }

        public bool IsPlanarFormat
        {
            get
            {
                return ChromaType == ChromaType.I420 ||
                       ChromaType == ChromaType.Nv12 ||
                       ChromaType == ChromaType.Yv12 ||
                       ChromaType == ChromaType.J420;
            }
        }

        public bool IsRgb
        {
            get
            {
                return ChromaType == ChromaType.Rv15 ||
                       ChromaType == ChromaType.Rv16 ||
                       ChromaType == ChromaType.Rv24 ||
                       ChromaType == ChromaType.Rv32 ||
                       ChromaType == ChromaType.Rgba;
            }
        }

        public int Planes { get; private set; }
        public int[] PlaneSizes { get; private set; }
        public int[] Pitches { get; private set; }
        public int[] Lines { get; private set; }
        public ChromaType ChromaType { get; private set; }
    }
}
