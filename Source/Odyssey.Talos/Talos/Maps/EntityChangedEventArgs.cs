namespace Odyssey.Talos.Maps
{

    public class EntityChangedEventArgs : EntityEventArgs
    {
        public IComponent Component { get; private set; }

        public EntityChangedEventArgs(IEntity source, IComponent component)
            : base(source)
        {
            Component = component;
        }
    }
}
