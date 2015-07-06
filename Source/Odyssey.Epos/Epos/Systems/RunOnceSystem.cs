using System.Collections.Generic;
using Odyssey.Epos.Messages;

namespace Odyssey.Epos.Systems
{
    public abstract class RunOnceSystem : UpdateableSystemBase
    {
        protected RunOnceSystem(Selector selector) : base(selector) { }

        public override void Start()
        {
            Subscribe<EntityChangeMessage>(EntityChangeMessage);
        }

        public override void Stop()
        {
            Unsubscribe<EntityChangeMessage>();
        }

        void EntityChangeMessage()
        {
            var mEntity = MessageQueue.Dequeue<EntityChangeMessage>();
            if (mEntity.Action == UpdateType.Add)
            {
                RegisterEntity(mEntity.Source);
            }
        }

        public override void AfterUpdate()
        {
            var entitiesCopy = new List<Entity>(Entities);

            foreach (Entity entity in entitiesCopy)
            {
                UnregisterEntity(entity);
            }

            IsEnabled = false;
        }
    }
}
