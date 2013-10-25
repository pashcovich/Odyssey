using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Devices
{
    public enum PointerDeviceType
    {
        Touch = 0,
        Pen = 1,
        Mouse = 2,
    }

    public sealed class PointerDevice
    {
        public PointerDeviceType PointerDeviceType { get; internal set; }

        internal PointerDevice() { }
    }
}
