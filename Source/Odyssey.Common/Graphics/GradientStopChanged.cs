using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics
{
    public class GradientStopChangedEventArgs : EventArgs
    {
        public GradientStop GradientStop { get; private set; }

        public GradientStopChangedEventArgs(GradientStop gradientStop)
        {
            GradientStop = gradientStop;
        }
    }
}
