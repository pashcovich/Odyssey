namespace Odyssey.Epos.Messages
{
    public class ComponentMessage<TComponent> : Message
        where TComponent : IComponent
    {
        public UpdateType Action { get; private set; }
        public TComponent Content { get; private set; }

        public ComponentMessage(TComponent content, UpdateType action, bool isSynchronous = false)
            : base(isSynchronous)
        {
            Action = action;
            Content = content;
        }
    }
}
