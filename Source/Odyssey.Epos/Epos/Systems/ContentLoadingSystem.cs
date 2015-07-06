using System.Linq;
using Odyssey.Engine;
using Odyssey.Epos.Components;
using Odyssey.Epos.Messages;
using Odyssey.Reflection;

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
            Subscribe<PropertyChangeMessage>(PropertyChangeMessage);
        }

        public override void Stop()
        {
            Unsubscribe<PropertyChangeMessage>();
        }

        void PropertyChangeMessage()
        {
            var cPropertyChange = MessageQueue.Dequeue<PropertyChangeMessage>();
            var attributes = ReflectionHelper.GetPropertyAttributes<PropertyUpdateAttribute>(cPropertyChange.Component.GetType(), cPropertyChange.Property);

            RegisterEntity(cPropertyChange.Component.Owner);

            //foreach (var attribute in attributes)
            //{
            //    switch (attribute.UpdateAction)
            //    {
            //        case UpdateAction.Register:
            //            RegisterEntity(cPropertyChange.Component.Owner);
            //            break;

            //        case UpdateAction.Initialize:
            //            ((IInitializable)cPropertyChange.Component).Initialize();
            //            break;
            //    }
            //}
            IsEnabled = true;
        }

        static void SetupEntity(IEntity entity)
        {
            var components = entity.Components.OfType<ContentComponent>().ToArray();
            bool ready = true;
            foreach (var component in components.Where(component => !component.IsInited))
            {
                component.Initialize();
                ready &= component.IsInited;
            }
            //entity.IsEnabled = ready;
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
