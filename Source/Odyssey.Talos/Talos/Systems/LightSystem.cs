using System.Linq;
using Odyssey.Engine;
using Odyssey.Talos.Components;
using Odyssey.Talos.Messages;
using Odyssey.Talos.Nodes;
using System.Collections.Generic;

namespace Odyssey.Talos.Systems
{
    public abstract class LightSystem : UpdateableSystemBase
    {
        readonly LightCollection lightNodes;
        protected ComponentType PositionComponentType { get; private set; }
        protected ComponentType UpdateComponentType { get; private set; }

        protected LightCollection LightNodes { get { return lightNodes; } }

        protected LightSystem(Aspect aspect) : base(aspect)
        {
            PositionComponentType = ComponentTypeManager.GetType<PositionComponent>();
            UpdateComponentType = ComponentTypeManager.GetType<UpdateComponent>();
            lightNodes = new LightCollection();
            
        }

        protected void RemoveEntity(IEntity entity)
        {
            var node = (from kvp in lightNodes where kvp.Value.EntityId == entity.Id select kvp.Value).First();
            lightNodes.Remove(node.Id);
            Messenger.Send(new LightMessage(entity, node, ChangeType.Removed));
        }

        public override void Start()
        {
            base.Start();
            Messenger.Register<ContentLoadedMessage<ShaderComponent>>(this);
            Messenger.Register<EntityChangeMessage>(this);
            Services.AddService(typeof(ILightService), lightNodes);
        }

        public override void Stop()
        {
            base.Stop();
            Messenger.Unregister<ContentLoadedMessage<ShaderComponent>>(this);
            Messenger.Unregister<EntityChangeMessage>(this);
        }

    }
}
