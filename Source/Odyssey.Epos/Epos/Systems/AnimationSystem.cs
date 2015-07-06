using System.Linq;
using Odyssey.Epos.Components;
using Odyssey.Epos.Messages;

namespace Odyssey.Epos.Systems
{
    public class AnimationSystem : UpdateableSystemBase
    {
        public AnimationSystem() : base(Selector.All(typeof(AnimationComponent)))
        { }

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
            // Entity change
            while (MessageQueue.HasItems<EntityChangeMessage>())
            {
                var mEntity = MessageQueue.Dequeue<EntityChangeMessage>();
                var entity = mEntity.Source;
                var cAnimation = entity.GetComponent<AnimationComponent>();
                cAnimation.Initialize();
            }
            return base.BeforeUpdate();
        }

        public override void Process(Engine.ITimeService time)
        {
            foreach (Entity entity in Entities)
            {
                var cAnimation = entity.GetComponent<AnimationComponent>();
                cAnimation.Controller.Update(time);
            }
        }
    }
}
