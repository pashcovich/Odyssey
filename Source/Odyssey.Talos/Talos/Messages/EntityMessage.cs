
namespace Odyssey.Talos.Messages
{
    public abstract class EntityMessage : Message
    {
        public IEntity Source { get; private set; }

        protected EntityMessage(IEntity source, bool isSynchronous = false)
            : base(isSynchronous)
        {
            Source = source;
        }
    }

}
