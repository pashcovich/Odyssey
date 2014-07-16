using System.Collections.Generic;
using Odyssey.Engine;
using SharpDX;

namespace Odyssey.Talos
{
    public interface IScene
    {
        IServiceRegistry Services { get; }
        void Update(ITimeService time);

        IEnumerable<IEntity> Entities { get; }
        IEnumerable<IComponent> Components{ get; }

        IEnumerable<TComponent> SelectComponents<TComponent>()
            where TComponent : IComponent;
        IEnumerable<IComponent> SelectEntityComponents(IEntity entity);
        bool TryGetEntityComponent<TComponent>(IEntity entity, long keyPart, out TComponent component)
            where TComponent : IComponent;
        TComponent GetEntityComponent<TComponent>(IEntity entity, long keyPart)
            where TComponent : IComponent;
        bool EntityHasComponent(IEntity entity, long keyPart);
        bool ContainsEntity(IEntity entity);
        IEntity SelectEntity(long id);
        void BeginDesign();
        void EndDesign();
        void Unload();

        bool IsFirstUpdateDone { get; }

        void Render(ITimeService appTime);
    }
}
