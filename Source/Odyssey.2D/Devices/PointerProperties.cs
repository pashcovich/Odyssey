using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Devices
{
    public sealed class PointerProperties
    {
        public bool IsLeftButtonPressed { get; internal set; }
        public bool IsRightButtonPressed { get; internal set; }
        public int MouseWheelDelta { get; internal set; }

        internal PointerProperties() { }
    }
}
