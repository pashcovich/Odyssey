using Odyssey.Engine;
using Odyssey.Talos.Components;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Systems
{
    [YamlTag("RotationSystem")]
    public class RotationSystem : UpdateableSystemBase, IUpdateableSystem
    {
        private readonly ComponentType tRotation;
        protected ComponentType Rotation { get { return tRotation; } }

        public RotationSystem() : base(Aspect.All(typeof (RotationComponent), typeof (UpdateComponent)))
        {
            tRotation = ComponentTypeManager.GetType<RotationComponent>();
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

                RotationComponent cRotation = entity.GetComponent<RotationComponent>(Rotation.KeyPart);
                cRotation.Orientation *= cRotation.Delta;
            }
        }
    }
}
