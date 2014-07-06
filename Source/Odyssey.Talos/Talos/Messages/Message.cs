
namespace Odyssey.Talos.Messages
{
    public abstract class Message : IMessage
    {
        public bool IsSynchronous { get; private set; }

        public Message(bool isSynchronous)
        {
            IsSynchronous = isSynchronous;
        }
    }
}
