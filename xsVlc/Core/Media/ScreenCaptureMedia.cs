//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using xsVlc.Common.Media;

namespace xsVlc.Core.Media
{
    internal class ScreenCaptureMedia : BasicMedia, IScreenCaptureMedia
    {
        private Rectangle _captureArea;
        private int _fps;
        private bool _followMouse;
        private string _cursorFile;

        public ScreenCaptureMedia(IntPtr hMediaLib) : base(hMediaLib)
        {
            _captureArea = Screen.PrimaryScreen.Bounds;
            _fps = 1;
        }

        public Rectangle CaptureArea
        {
            get
            {
                return _captureArea;
            }
            set
            {
                _captureArea = value;
                UpdateCaptureArea();
            }
        }

        private void UpdateCaptureArea()
        {
            var options = new List<string>
                              {
                                  string.Format(":screen-top={0}", _captureArea.Top),
                                  string.Format(":screen-left={0}", _captureArea.Left),
                                  string.Format(":screen-width={0}", _captureArea.Width),
                                  string.Format(":screen-height={0}", _captureArea.Height)
                              };
            AddOptions(options);
        }

        public int Fps
        {
            get
            {
                return _fps;
            }
            set
            {
                _fps = value;
                UpdateFrameRate();
            }
        }

        private void UpdateFrameRate()
        {
            var options = new List<string>
                              {
                                  string.Format(":screen-fps={0}", _fps)
                              };
            AddOptions(options);
        }

        private void UpdateFollowMouse()
        {
            var options = new List<string>
                              {
                                  string.Format(":screen-follow-mouse={0}", _followMouse.ToString())
                              };

            AddOptions(options);
        }

        public bool FollowMouse
        {
            get
            {
                return _followMouse;
            }
            set
            {
                _followMouse = value;
                UpdateFollowMouse();
            }
        }

        public string CursorFile
        {
            get
            {
                return _cursorFile;
            }
            set
            {
                _cursorFile = value;
                UpdateCursorImage();
            }
        }

        private void UpdateCursorImage()
        {
            var options = new List<string>
                              {
                                  string.Format(":screen-mouse-image={0}", _cursorFile)
                              };
            AddOptions(options);
        }
    }
}
