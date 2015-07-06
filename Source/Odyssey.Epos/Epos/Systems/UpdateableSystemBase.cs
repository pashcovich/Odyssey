using Odyssey.Engine;
using Odyssey.Epos.Components;

namespace Odyssey.Epos.Systems
{
    public abstract class UpdateableSystemBase : SystemBase, IUpdateableSystem
    {
        private readonly ComponentType tUpdate;
        protected ComponentType Update { get { return tUpdate; } }
        

        protected UpdateableSystemBase(Selector selector) : base(selector)
        {
            tUpdate = ComponentTypeManager.GetType<UpdateComponent>();
        }

        public virtual bool BeforeUpdate()
        {
            HandleMessages();
            return IsEnabled && HasEntities;
        }

        public virtual void AfterUpdate()
        {

        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public abstract void Process(ITimeService time);
    }
}