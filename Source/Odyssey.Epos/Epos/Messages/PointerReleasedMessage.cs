using Odyssey.Interaction;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Messages
{
    public class PointerReleasedMessage : PointerMovedMessage
    {
        public MouseButtons MouseButtons { get; private set; }

        public PointerReleasedMessage(Vector2 location, MouseButtons mouseButtons, INotifiable sender)
            : base(location, sender)
        {
            MouseButtons = mouseButtons;
        }
    }
}
