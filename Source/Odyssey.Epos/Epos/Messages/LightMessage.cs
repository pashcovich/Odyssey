using Odyssey.Epos.Nodes;

namespace Odyssey.Epos.Messages
{
    public class LightMessage: EntityChangeMessage
    {
        public LightNode Light { get; private set; }

        public LightMessage(Entity source, LightNode light, UpdateType action, bool isSynchronous = false)
            : base(source, action, isSynchronous)
        {
            Light = light;
        }
    }
}
