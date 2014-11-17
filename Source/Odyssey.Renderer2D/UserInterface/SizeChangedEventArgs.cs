using System;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface
{
    public class SizeChangedEventArgs :EventArgs
    {
        public Vector2 OldSize { get; private set; }
        public Vector2 NewSize { get; private set; }

        public SizeChangedEventArgs(Vector2 oldSize, Vector2 newSize)
        {
            OldSize = oldSize;
            NewSize = newSize;
        }
    }
}
