using System;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface
{
    public class SizeChangedEventArgs :EventArgs
    {
        public Vector3 OldSize { get; private set; }
        public Vector3 NewSize { get; private set; }

        public SizeChangedEventArgs(Vector3 oldSize, Vector3 newSize)
        {
            OldSize = oldSize;
            NewSize = newSize;
        }
    }
}
