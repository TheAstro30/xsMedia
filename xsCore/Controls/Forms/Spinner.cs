/* xsMedia - sxCore
 * (c)2013 - 2025
 * Jason James Newland
 * KangaSoft Software, All Rights Reserved
 * Licenced under the GNU public licence */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace xsCore.Controls.Forms
{
    public class Spinner : Control
    {
        private const double NumberOfDegreesInCircle = 360;
        private const double NumberOfDegreesInHalfCircle = NumberOfDegreesInCircle / 2;
        private const int DefaultInnerCircleRadius = 8;
        private const int DefaultOuterCircleRadius = 10;
        private const int DefaultNumberOfSpoke = 10;
        private const int DefaultSpokeThickness = 4;

        private readonly Timer _timer;
        private bool _timerActive;
        private int _numSpokes;
        private int _spokeThickness;
        private int _progressValue;
        private int _outerCircleRadius;
        private int _innerCircleRadius;
        private PointF _centerPoint;
        private Color _color;
        private Color[] _colors;
        private double[] _angles;

        public Spinner()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);

            SetCircleAppearance(12, 2, 5, 11);

            _color = Color.DarkGray;

            GenerateColorsPallet();
            GetSpokesAngles();
            GetControlCenterPoint();

            _timer = new Timer();
            _timer.Tick += TimerTick;
            ActiveTimer();
        }

        /* Properties */
        public bool SpinnerActive
        {
            get
            {
                return _timerActive;
            }
            set
            {
                _timerActive = value;
                ActiveTimer();
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;

                GenerateColorsPallet();
                Invalidate();
            }
        }

        public int OuterCircleRadius
        {
            get
            {
                if (_outerCircleRadius == 0)
                {
                    _outerCircleRadius = DefaultOuterCircleRadius;
                }
                return _outerCircleRadius;
            }
            set
            {
                _outerCircleRadius = value;
                Invalidate();
            }
        }

        public int InnerCircleRadius
        {
            get
            {
                if (_innerCircleRadius == 0)
                {
                    _innerCircleRadius = DefaultInnerCircleRadius;
                }
                return _innerCircleRadius;
            }
            set
            {
                _innerCircleRadius = value;
                Invalidate();
            }
        }

        public int NumberOfSpokes
        {
            get
            {
                if (_numSpokes == 0)
                {
                    _numSpokes = DefaultNumberOfSpoke;
                }
                return _numSpokes;
            }
            set
            {
                if (_numSpokes == value || _numSpokes <= 0) { return; }
                _numSpokes = value;
                GenerateColorsPallet();
                GetSpokesAngles();
                Invalidate();
            }
        }

        public int SpokeThickness
        {
            get
            {
                if (_spokeThickness <= 0)
                {
                    _spokeThickness = DefaultSpokeThickness;
                }
                return _spokeThickness;
            }
            set
            {
                _spokeThickness = value;
                Invalidate();
            }
        }

        public int RotationSpeed
        {
            get
            {
                return _timer.Interval;
            }
            set
            {
                if (value > 0)
                {
                    _timer.Interval = value;
                }
            }
        }

        public void SetCircleAppearance(int numberSpoke, int spokeThickness, int innerCircleRadius, int outerCircleRadius)
        {
            NumberOfSpokes = numberSpoke;
            SpokeThickness = spokeThickness;
            InnerCircleRadius = innerCircleRadius;
            OuterCircleRadius = outerCircleRadius;

            Invalidate();
        }

        /* Overrides */
        protected override void OnPaint(PaintEventArgs e)
        {
            if (SpinnerActive && _numSpokes > 0)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                var intPosition = _progressValue;
                for (var intCounter = 0; intCounter < _numSpokes; intCounter++)
                {
                    intPosition = intPosition % _numSpokes;
                    DrawLine(e.Graphics,
                             GetCoordinate(_centerPoint, _innerCircleRadius, _angles[intPosition]),
                             GetCoordinate(_centerPoint, _outerCircleRadius, _angles[intPosition]),
                             _colors[intCounter], _spokeThickness);
                    intPosition++;
                }
            }
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GetControlCenterPoint();
            base.OnResize(e);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            proposedSize.Width = (_outerCircleRadius + _spokeThickness) * 2;
            return proposedSize;
        }

        /* Private methods */
        private void ActiveTimer()
        {
            if (_timerActive)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
                _progressValue = 0;
            }
            GenerateColorsPallet();
            Invalidate();
        }

        private static void DrawLine(Graphics g, PointF startPoint, PointF endPoint, Color color, int lineThickness)
        {
            using (var pen = new Pen(new SolidBrush(color), lineThickness))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                g.DrawLine(pen, startPoint, endPoint);
            }
        }

        private static Color Darken(Color color, int percent)
        {
            int intRed = color.R;
            int intGreen = color.G;
            int intBlue = color.B;
            return Color.FromArgb(percent, Math.Min(intRed, byte.MaxValue), Math.Min(intGreen, byte.MaxValue), Math.Min(intBlue, byte.MaxValue));
        }

        private void GenerateColorsPallet()
        {
            _colors = GenerateColorsPallet(_color, SpinnerActive, _numSpokes);
        }

        private Color[] GenerateColorsPallet(Color color, bool shadeColor, int numSpokes)
        {
            var objColors = new Color[NumberOfSpokes];
            /* Value is used to simulate a gradient feel... for each spoke, the 
             * color will be darken by value in intIncrement. */
            var bytIncrement = (byte)(byte.MaxValue / NumberOfSpokes);
            /* Reset variable in case of multiple passes */
            byte percentageOfDarken = 0;
            for (var intCursor = 0; intCursor < NumberOfSpokes; intCursor++)
            {
                if (shadeColor)
                {
                    if (intCursor == 0 || intCursor < NumberOfSpokes - numSpokes)
                    {
                        objColors[intCursor] = color;
                    }
                    else
                    {
                        /* Increment alpha channel color */
                        percentageOfDarken += bytIncrement;
                        /* Determine the spoke forecolor */
                        objColors[intCursor] = Darken(color, percentageOfDarken);
                    }
                }
                else
                {
                    objColors[intCursor] = color;
                }
            }
            return objColors;
        }

        private void GetControlCenterPoint()
        {
            _centerPoint = GetControlCenterPoint(this);
        }

        private static PointF GetControlCenterPoint(Control control)
        {
            var width = control.Width / 2;
            var height = (control.Height / 2) - 1;
            return new PointF(width, height);
        }

        private static PointF GetCoordinate(PointF circleCenter, int radius, double angle)
        {
            var dblAngle = Math.PI * angle / NumberOfDegreesInHalfCircle;
            return new PointF(circleCenter.X + radius * (float)Math.Cos(dblAngle),
                              circleCenter.Y + radius * (float)Math.Sin(dblAngle));
        }

        private void GetSpokesAngles()
        {
            _angles = GetSpokesAngles(NumberOfSpokes);
        }

        private static double[] GetSpokesAngles(int numSpokes)
        {
            var angles = new double[numSpokes];
            var angle = NumberOfDegreesInCircle / numSpokes;
            for (var shtCounter = 0; shtCounter < numSpokes; shtCounter++)
            {
                angles[shtCounter] = (shtCounter == 0 ? angle : angles[shtCounter - 1] + angle);
            }
            return angles;
        }

        /* Animation timer */
        private void TimerTick(object sender, EventArgs e)
        {
            _progressValue = ++_progressValue % _numSpokes;
            Invalidate();
        }
    }
}