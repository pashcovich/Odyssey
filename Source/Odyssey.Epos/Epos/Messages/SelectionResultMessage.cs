using SharpDX;

namespace Odyssey.Epos.Messages
{
    public class SelectionResultMessage : EntityMessage
    {
        public Entity Result { get; private set; }

        public SelectionResultMessage(Entity source, bool isSynchronous = false) : base(source, isSynchronous)
        {
        }
    }
}
