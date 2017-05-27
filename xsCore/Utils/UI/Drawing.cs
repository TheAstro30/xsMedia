using System.Drawing;

namespace xsCore.Utils.UI
{
    public sealed class Drawing
    {
        /* Resize a bitmap with aspect */
        public static Size ResizeBitmapWithAspect(Size clientSize, Size bitmapSize)
        {
            return ResizeBitmapWithAspect(clientSize, bitmapSize, Size.Empty);
        }

        public static Size ResizeBitmapWithAspect(Size clientSize, Size bitmapSize, Size maxSize)
        {
            var ratioX = (double)clientSize.Width / bitmapSize.Width;
            var ratioY = (double)clientSize.Height / bitmapSize.Height;
            /* Use which-ever is the greatest */
            var ratio = ratioX > ratioY ? ratioY : ratioX;

            var newWidth = (float)ratio * bitmapSize.Width;
            var newHeight = (float)ratio * bitmapSize.Height;
            /* If maxSize isn't empty, make sure image never exceeds it */
            if (maxSize != Size.Empty && (newWidth > maxSize.Width || newHeight > maxSize.Height))
            {                
                return maxSize;
            }
            return new Size((int)newWidth, (int)newHeight);
        }

        /* Draw rounded rectangle */
        public static void DrawRoundedRectangle(Graphics g, Rectangle rect, int diameter, Color color)
        {
            DrawRoundedRectangle(g, rect.X, rect.Y, rect.Width, rect.Height, diameter, color);
        }

        public static void DrawRoundedRectangle(Graphics g, int x, int y, int width, int height, int diameter, Color color)
        {
            using (var pen = new Pen(color))
            {
                var baseRect = new RectangleF(x, y, width, height);
                var arcRect = new RectangleF(baseRect.Location, new SizeF(diameter, diameter));
                /* Top left arc */
                g.DrawArc(pen, arcRect, 180, 90);
                g.DrawLine(pen, x + (int)(diameter / 2.0), y,
                                     x + width - (int)(diameter / 2.0), y);
                /* Top right arc */
                arcRect.X = baseRect.Right - diameter;
                g.DrawArc(pen, arcRect, 270, 90);
                g.DrawLine(pen, x + width, y + (int)(diameter / 2.0),
                                     x + width,
                                     y + height - (int)(diameter / 2.0));
                /* Bottom right arc */
                arcRect.Y = baseRect.Bottom - diameter;
                g.DrawArc(pen, arcRect, 0, 90);
                g.DrawLine(pen, x + (int)(diameter / 2.0), y + height,
                                     x + width - (int)(diameter / 2.0),
                                     y + height);
                /* Bottom left arc */
                arcRect.X = baseRect.Left;
                g.DrawArc(pen, arcRect, 90, 90);
                g.DrawLine(pen, x, y + (int)(diameter / 2.0), x,
                                     y + height - (int)(diameter / 2.0));
            }
        }
    }
}
