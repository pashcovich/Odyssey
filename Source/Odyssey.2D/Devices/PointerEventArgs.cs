using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Devices
{
    public class PointerEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public PointerPoint CurrentPoint { get; private set; }

        public PointerEventArgs(PointerPoint point)
        {
            CurrentPoint = point;
        }

    }
}
