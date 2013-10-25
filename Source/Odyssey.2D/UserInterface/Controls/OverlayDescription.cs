using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.UserInterface.Controls
{
    public struct OverlayDescription
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public OverlayDescription(int width, int height)
            : this()
        {
            Width = width;
            Height = height;
        }
    }
}
