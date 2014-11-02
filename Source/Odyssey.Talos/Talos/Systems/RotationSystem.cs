using Odyssey.Engine;
using Odyssey.Talos.Components;
using SharpDX;

namespace Odyssey.Talos.Systems
{
    public class RotationSystem : UpdateableSystemBase
    {
        public RotationSystem()
            : base(Selector.All(typeof(UpdateComponent), typeof (OrientationComponent), typeof (RotationComponent)))
        {
        }

        public override void Process(ITimeService time)
        {
            foreach (var entity in Entities)
            {
                var cRotation = entity.GetComponent<RotationComponent>();
                if (cRotation.AngularVelocity == Vector3.Zero)
                    continue;
                var cOrientation = entity.GetComponent<OrientationComponent>();
                Quaternion w = new Quaternion(cRotation.AngularVelocity, 0) ;
                cOrientation.Orientation.Normalize();
                w = 0.5f*w*cOrientation.Orientation;
                cOrientation.Orientation += w * time.FrameTime;
                
            }
        }
    }
}
