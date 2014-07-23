using Odyssey.Graphics.Shaders;
using Odyssey.Talos.Components;
namespace Odyssey.Talos.Messages
{
    public class ComponentMessage<TComponent> : Message
        where TComponent : IComponent
    {
        public ChangeType Action { get; private set; }
        public TComponent Content { get; private set; }

        public ComponentMessage(TComponent content, ChangeType action, bool isSynchronous = false)
            : base(isSynchronous)
        {
            Action = action;
            Content = content;
        }
    }
}
