
namespace Odyssey.Epos.Messages
{
    public abstract class Message
    {
        public bool IsBlocking { get; private set; }

        protected Message(bool isBlocking)
        {
            IsBlocking = isBlocking;
        }
    }
}
