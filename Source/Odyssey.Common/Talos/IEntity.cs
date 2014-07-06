using System.Collections.Generic;

namespace Odyssey.Talos
{
    public interface IEntity
    {
        long Id { get; }
        long Key { get; }
        bool IsEnabled { get; set; }
        bool ContainsComponent<TComponent>()
            where TComponent : IComponent;
        bool ContainsComponent(long keyPart);
        string Name { get; set; }
        IEnumerable<IComponent> Components { get; }
        bool TryGetComponent<TComponent>(long keypart, out TComponent component)
            where TComponent : IComponent;
        TComponent GetComponent<TComponent>(long keyPart)
            where TComponent : IComponent;
        TComponent GetComponent<TComponent>()
            where TComponent : IComponent;

        IComponent GetComponent(string componentType);
        IScene Scene { get;}
        bool Validate();
    }
}
