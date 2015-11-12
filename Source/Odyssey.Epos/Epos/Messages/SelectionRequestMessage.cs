using SharpDX;

namespace Odyssey.Epos.Messages
{
    public class SelectionRequestMessage : Message
    {
        public Vector2 Location { get; private set; }
        public INotifiable Sender { get; private set; }

        public SelectionRequestMessage(Vector2 location, INotifiable sender)
            : base(false)
        {
            Location = location;
            Sender = sender;
        }
    }
}
