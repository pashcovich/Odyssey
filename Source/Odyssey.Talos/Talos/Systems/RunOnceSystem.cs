using System.Collections.Generic;
using Odyssey.Talos.Messages;

namespace Odyssey.Talos.Systems
{
    public abstract class RunOnceSystem : UpdateableSystemBase
    {
        protected RunOnceSystem(Selector selector) : base(selector) { }

        public override void Start()
        {
            Messenger.Register<EntityChangeMessage>(this);
        }

        public override void Stop()
        {
            Messenger.Unregister<EntityChangeMessage>(this);
        }

        public override bool BeforeUpdate()
        {
            while (MessageQueue.HasItems<EntityChangeMessage>())
            {
                var mEntity = MessageQueue.Dequeue<EntityChangeMessage>();
                if (mEntity.Action == UpdateType.Add)
                {
                    RegisterEntity(mEntity.Source);
                }
            }

            return base.BeforeUpdate();
        }

        public override void AfterUpdate()
        {
            var entitiesCopy = new List<Entity>(Entities);

            foreach (Entity entity in entitiesCopy)
            {
                Scene.SystemMap.UnregisterEntityFromSystem(entity, this);
            }

            IsEnabled = false;
        }
    }
}
