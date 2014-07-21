using System.Linq;
using Odyssey.Engine;
using Odyssey.Talos.Components;
using Odyssey.Talos.Initializers;
using Odyssey.Talos.Messages;
using Odyssey.Talos.Nodes;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Systems
{
    [YamlTag("PointLightSystem")]
    public sealed class PointLightSystem : LightSystem
    {
        public PointLightSystem()
            : base(Selector.All(typeof(PositionComponent), typeof(PointLightComponent), typeof(UpdateComponent)))
        {
        }

        void SetupEntity(IEntity entity)
        {
            PointLightNode nPointLight = new PointLightNode(entity);
            LightNodes.Add(nPointLight.Id, nPointLight);
            Messenger.Send(new LightMessage(entity, nPointLight, ChangeType.Removed));
        }

        public override void BeforeUpdate()
        {
            // Entity change
            while (MessageQueue.HasItems<EntityChangeMessage>())
            {
                EntityChangeMessage mEntity = MessageQueue.Dequeue<EntityChangeMessage>();
                if (mEntity.Action == ChangeType.Added)
                    SetupEntity(mEntity.Source);
                else if (mEntity.Action == ChangeType.Removed)
                    RemoveEntity(mEntity.Source);
            }

            // Set up shader
            //while (MessageQueue.HasItems<ContentLoadedMessage<ShaderComponent>>())
            //{
            //    var mShader = MessageQueue.Dequeue<ContentLoadedMessage<ShaderComponent>>();
            //    ShaderInitializer sInitializer = new ShaderInitializer(Scene.Services, mShader.Content.Technique.Effect, mShader.Content.Technique.ActiveTechnique);
            //    // TODO improve Light System
            //    sInitializer.InitializeLight(LightNodes.Values.First());
            //}
        }


        public override void Process(ITimeService time)
        {
            foreach (IEntity entity in Entities)
            {
                if (!entity.IsEnabled)
                    continue;
                var cUpdate = entity.GetComponent<UpdateComponent>(Update.KeyPart);
                if (!cUpdate.RequiresUpdate)
                    continue;

                LightNode lightNode = (from kvp in LightNodes
                    where kvp.Value.EntityId == entity.Id
                    select kvp.Value).First();

                lightNode.Update();
            }
        }
    }
}
