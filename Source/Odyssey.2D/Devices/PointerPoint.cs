using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Devices
{
    public sealed class PointerPoint
    {
        public bool IsInContact { get; internal set; }
        public uint PointerId { get; private set;}
        public Vector2 Position { get; internal set; }
        public TimeSpan TimeStamp { get; private set; }
        public PointerProperties Properties { get; private set;}
        public PointerDevice PointerDevice { get; private set; }

        internal PointerPoint(uint pointerId, PointerProperties properties, PointerDevice pointerDevice)
        {
            TimeStamp = DateTime.Now.TimeOfDay;
            PointerId = pointerId;
            Properties = properties;
        }
    }
}
