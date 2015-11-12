using System;
using SharpDX;

namespace Odyssey.UserInterface.Events
{
    public class PositionChangedEventArgs :EventArgs
    {
        public Vector3 OldPosition { get; private set; }
        public Vector3 NewPosition { get; private set; }

        public PositionChangedEventArgs(Vector3 oldPosition, Vector3 newPosition)
        {
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}
