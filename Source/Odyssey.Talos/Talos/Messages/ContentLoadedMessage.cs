using Odyssey.Talos.Components;
namespace Odyssey.Talos.Messages
{
    public class ContentLoadedMessage<TComponent> : Message
        where TComponent : ContentComponent
    {
        public TComponent Content { get; private set; }

        public ContentLoadedMessage(TComponent shader, bool isSynchronous = false)
            : base(isSynchronous)
        {
            Content = shader;
        }
    }
}
