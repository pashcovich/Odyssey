using System;
using Odyssey.Geometry;
using Odyssey.Talos.Components;
using SharpDX;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Systems
{
    [YamlTag("OrbitSystem")]
    public class OrbitSystem : UpdateableSystemBase
    {
        readonly ComponentType tOrbitBehaviour;
        readonly ComponentType tPosition;
        private readonly ComponentType tParent;

        public OrbitSystem() : base(Selector.All(typeof (OrbitBehaviourComponent), typeof (UpdateComponent))
            .GetOne(typeof (RotationComponent), typeof (ParentComponent)))
        {
            tOrbitBehaviour = ComponentTypeManager.GetType<OrbitBehaviourComponent>();
            tPosition = ComponentTypeManager.GetType<PositionComponent>();
            tParent = ComponentTypeManager.GetType<ParentComponent>();
        }

        public override void Process(Engine.ITimeService time)
        {
            foreach (IEntity entity in Entities)
            {
                if (!entity.IsEnabled)
                    continue;

                var cUpdate = entity.GetComponent<UpdateComponent>(Update.KeyPart);
                if (!cUpdate.RequiresUpdate)
                    continue;

                var cOrbit = entity.GetComponent<OrbitBehaviourComponent>(tOrbitBehaviour.KeyPart);
                var cPosition = entity.GetComponent<PositionComponent>(tPosition.KeyPart);

                ParentComponent cParent;
                Vector3 center = entity.TryGetComponent(out cParent)
                    ? cParent.Entity.GetComponent<PositionComponent>(tPosition.KeyPart).Position
                    : Vector3.Zero;

                float x = cOrbit.RadiusX * (float)Math.Cos(cOrbit.Theta);
                const float y = 0;
                float z = cOrbit.RadiusZ * (float)Math.Sin(cOrbit.Theta);

                cOrbit.Theta += (float)((cOrbit.Speed * time.ElapsedApplicationTime.TotalSeconds) % MathHelper.TwoPi);
                
                cPosition.Position = new Vector3(x,y,z);

            }

        }
    }
}
