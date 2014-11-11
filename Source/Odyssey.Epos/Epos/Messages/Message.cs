
namespace Odyssey.Epos.Messages
{
    public abstract class Message
    {
        public bool IsBlocking { get; private set; }

        public Message(bool isBlocking)
        {
            IsBlocking = isBlocking;
        }
    }
}
