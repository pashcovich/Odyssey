using Odyssey.Interaction;
using SharpDX;

namespace Odyssey.Epos.Messages
{
    public class PointerPressedMessage : PointerMovedMessage
    {
        public MouseButtons MouseButtons { get; private set; }

        public PointerPressedMessage(Vector2 location, MouseButtons mouseButtons, INotifiable sender) : base(location, sender)
        {
            MouseButtons = mouseButtons;
        }
    }
}
