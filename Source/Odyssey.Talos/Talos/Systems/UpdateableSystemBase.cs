using Odyssey.Engine;
using Odyssey.Talos.Components;

namespace Odyssey.Talos.Systems
{
    public abstract class UpdateableSystemBase : SystemBase, IUpdateableSystem
    {
        private readonly ComponentType tUpdate;
        protected ComponentType Update { get { return tUpdate; } }
        

        protected UpdateableSystemBase(Aspect aspect) : base(aspect)
        {
            tUpdate = ComponentTypeManager.GetType<UpdateComponent>();
        }

        public virtual void BeforeUpdate()
        {
        }

        public virtual void AfterUpdate()
        {
            foreach (IEntity entity in Scene.SystemMap.SelectAllEntities(this))
            {
                if (!entity.IsEnabled)
                    continue;

                var cUpdate = entity.GetComponent<UpdateComponent>(tUpdate.KeyPart);
                if (cUpdate.UpdateFrequency == UpdateFrequency.Static)
                    cUpdate.RequiresUpdate = false;
                else if (cUpdate.UpdateFrequency == UpdateFrequency.RealTime)
                    cUpdate.RequiresUpdate = true;
            }
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