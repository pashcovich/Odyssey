using System.Collections.Generic;

namespace Odyssey.Talos
{
    public interface IEntity
    {
        /// <summary>
        /// Returns the sequential Id number of this instance.
        /// </summary>
        long Id { get; }
        /// <summary>
        /// Returns a value indicating the combination of components registered to this instance.
        /// </summary>
        long Key { get; }
        bool IsEnabled { get; set; }
        bool ContainsComponent<TComponent>()
            where TComponent : IComponent;
        bool ContainsComponent(long keyPart);
        string Name { get; set; }
        IEnumerable<IComponent> Components { get; }
        bool TryGetComponent<TComponent>(out TComponent component)
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
