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
        private readonly PointerDevice pointerDevice;
        private readonly PointerProperties properties;
        private readonly uint pointerId;
        private readonly TimeSpan timeStamp;
        public bool IsInContact { get; internal set; }

        public uint PointerId
        {
            get { return pointerId; }
        }

        public Vector2 Position { get; internal set; }

        public TimeSpan TimeStamp
        {
            get { return timeStamp; }
        }

        public PointerProperties Properties
        {
            get { return properties; }
        }

        public PointerDevice PointerDevice
        {
            get { return pointerDevice; }
        }

        internal PointerPoint(uint pointerId, PointerProperties properties, PointerDevice pointerDevice)
        {
            timeStamp = DateTime.Now.TimeOfDay;
            this.pointerId = pointerId;
            this.properties = properties;
            this.pointerDevice = pointerDevice;
        }
    }
}
