using SharpDX;

namespace Odyssey.Epos.Messages
{
    public class PointerMovedMessage : Message
    {
        public Vector2 Location { get; private set; }
        public INotifiable Sender { get; private set; }

        public PointerMovedMessage(Vector2 location, INotifiable sender)
            : base(false)
        {
            Location = location;
            Sender = sender;
        }
    }
}
