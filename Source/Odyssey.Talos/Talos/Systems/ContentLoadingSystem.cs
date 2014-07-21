using System.Collections.Generic;
using Odyssey.Engine;
using Odyssey.Talos.Components;
using Odyssey.Talos.Messages;

namespace Odyssey.Talos.Systems
{
    public class ContentLoadingSystem<TComponent> : SystemBase, IUpdateableSystem
        where TComponent : ContentComponent
    {
        public ContentLoadingSystem() : base(Selector.All(typeof(TComponent)))
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
            var component = entity.GetComponent<TComponent>();
            if (!component.IsInited)
            {
                component.Initialize();
                var mContent = new ContentLoadedMessage<TComponent>(component);
                Messenger.Send(mContent);
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
