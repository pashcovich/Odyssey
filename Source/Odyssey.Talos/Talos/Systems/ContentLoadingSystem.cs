using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Odyssey.Engine;
using Odyssey.Talos.Components;
using Odyssey.Talos.Messages;

namespace Odyssey.Talos.Systems
{
    public class ContentLoadingSystem : SystemBase, IUpdateableSystem
    {
        public ContentLoadingSystem() : base(Selector.One(typeof(ShaderComponent),
            typeof(PostProcessComponent), typeof(ModelComponent), typeof(DiffuseMappingComponent)))
        { 
        }

        public override void Start()
        {
            Messenger.Register<PropertyChangeMessage>(this);
            Messenger.Register<EntityChangeMessage>(this);
        }

        public override void Stop()
        {
            Messenger.Unregister<PropertyChangeMessage>(this);
            Messenger.Unregister<EntityChangeMessage>(this);
        }

        void SetupEntity(IEntity entity)
        {
            var components = entity.Components.OfType<ContentComponent>();

            foreach (var component in components.Where(component => !component.IsInited))
            {
                component.Initialize();
            }
        }

        public void BeforeUpdate()
        {
            while (MessageQueue.HasItems<EntityChangeMessage>())
            {
                var mEntity = MessageQueue.Dequeue<EntityChangeMessage>();
                if (mEntity.Action == ChangeType.Added)
                {
                    Scene.SystemMap.RegisterEntityToSystem(mEntity.Source, this);
                }
            }

            if (HasEntities)
                IsEnabled = true;
        }

        public void Process(ITimeService time)
        {
            foreach (IEntity entity in Entities)
            {
                SetupEntity(entity);
            }
        }

        public void AfterUpdate()
        {
            var entitiesCopy = new List<IEntity>(Entities);

            foreach (IEntity entity in entitiesCopy)
            {
                Scene.SystemMap.UnregisterEntityFromSystem(entity, this);
            }

            if (!HasEntities)
                IsEnabled = false;
        }
    }
}
