#region Using Directives

using Odyssey.Engine;
using Odyssey.Talos.Components;
using SharpDX;
using SharpYaml.Serialization;

#endregion

namespace Odyssey.Talos.Systems
{
    [YamlTag("PositionSystem")]
    public class PositionSystem : UpdateableSystemBase
    {
        private readonly ComponentType tParent;
        private readonly ComponentType tPosition;
        private readonly ComponentType tRotation;
        private readonly ComponentType tScaling;
        private readonly ComponentType tTransform;

        public PositionSystem() : base(Aspect.All(typeof (PositionComponent), typeof (UpdateComponent))
            .GetOne(typeof(TransformComponent), typeof (RotationComponent), typeof (ScalingComponent), typeof (ParentComponent)))
        {
            tPosition = ComponentTypeManager.GetType<PositionComponent>();
            tScaling = ComponentTypeManager.GetType<ScalingComponent>();
            tParent = ComponentTypeManager.GetType<ParentComponent>();
            tRotation = ComponentTypeManager.GetType<RotationComponent>();
            tTransform = ComponentTypeManager.GetType<TransformComponent>();
        }

        protected PositionSystem(Aspect aspect) : base(aspect)
        {
        }

        protected ComponentType Position
        {
            get { return tPosition; }
        }

        protected ComponentType Scaling
        {
            get { return tScaling; }
        }

        protected ComponentType Parent
        {
            get { return tParent; }
        }

        protected ComponentType Rotation
        {
            get { return tRotation; }
        }

        protected ComponentType Transform
        {
            get { return tTransform; }
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

                var cPosition = entity.GetComponent<PositionComponent>(Position.KeyPart);
                var cTransform = entity.GetComponent<TransformComponent>(Transform.KeyPart);
                ScalingComponent cScaling;
                RotationComponent cRotation;
                ParentComponent cParent;

                Matrix mLocalWorld = Matrix.Identity;

                if (entity.TryGetComponent(Scaling.KeyPart, out cScaling) && cScaling.Scaling != Vector3.Zero)
                {
                    cTransform.Scaling = Matrix.Scaling(cScaling.Scaling);
                    mLocalWorld *= cTransform.Scaling;
                }

                if (entity.TryGetComponent(Rotation.KeyPart, out cRotation) &&
                    cRotation.Orientation != Quaternion.Identity)
                {
                    cTransform.Rotation = Matrix.RotationQuaternion(cRotation.Orientation);
                    mLocalWorld *= cTransform.Rotation;
                }

                if (cPosition.Position != Vector3.Zero)
                {
                    cTransform.Translation = Matrix.Translation(cPosition.Position);
                    mLocalWorld *= cTransform.Translation;
                }

                cTransform.Local = mLocalWorld;

                if (entity.TryGetComponent(Parent.KeyPart, out cParent) && cParent.Entity != null)
                {
                    var cParentTransform = cParent.Entity.GetComponent<TransformComponent>();
                    //cTransform.World = cParentTransform.Rotation * cParentTransform.Translation * cTransform.Local;
                    cTransform.World = cTransform.Local*cParentTransform.World;
                }
                else
                    cTransform.World = cTransform.Local;
            }
        }
    }
}