using System.Linq;
using Odyssey.Engine;
using Odyssey.Epos.Components;
using Odyssey.Epos.Messages;
using Odyssey.Utilities.Reflection;

namespace Odyssey.Epos.Systems
{
    public class ContentLoadingSystem : RunOnceSystem
    {
        public ContentLoadingSystem() : base(Selector.One(typeof(ShaderComponent),
            typeof(PostProcessComponent), typeof(ModelComponent), typeof(DiffuseMappingComponent)))
        { 
        }

        public override void Start()
        {
            base.Start();
            Messenger.Register<PropertyChangeMessage>(this);
        }

        public override void Stop()
        {
            base.Stop();
            Messenger.Unregister<PropertyChangeMessage>(this);
        }

        public override bool BeforeUpdate()
        {
            while (MessageQueue.HasItems<PropertyChangeMessage>())
            {
                var cPropertyChange = MessageQueue.Dequeue<PropertyChangeMessage>();
                if (string.Equals(cPropertyChange.Property, ReflectionHelper.GetPropertyName((ContentComponent c)=> c.AssetName)))
                    RegisterEntity(cPropertyChange.Component.Owner);
            }
            return base.BeforeUpdate();
        }

        static void SetupEntity(IEntity entity)
        {
            var components = entity.Components.OfType<ContentComponent>();

            foreach (var component in components.Where(component => !component.IsInited))
            {
                component.Initialize();
            }
        }

        public override void Process(ITimeService time)
        {
            foreach (Entity entity in Entities)
            {
                SetupEntity(entity);
            }
        }


    }
}
