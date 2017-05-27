﻿using System;
using System.Drawing;

namespace xsTrackBar.TrackBarBase
{
    public delegate void TrackBarDrawItemEventHandler(object sender, TrackBarDrawItemEventArgs e);

    public class TrackBarDrawItemEventArgs : EventArgs
    {
        private readonly Rectangle _bounds;
        private readonly Graphics _graphics;
        private readonly TrackBarItemState _state;

        public TrackBarDrawItemEventArgs(Graphics graphics, Rectangle bounds, TrackBarItemState state)
        {
            _graphics = graphics;
            _bounds = bounds;
            _state = state;
        }

        public Rectangle Bounds
        {
            get
            {
                return _bounds;
            }
        }

        public Graphics Graphics
        {
            get
            {
                return _graphics;
            }
        }

        public TrackBarItemState State
        {
            get
            {
                return _state;
            }
        }
    }
}

