using Odyssey.Engine;
using Odyssey.Epos.Components;
using SharpDX.Mathematics;

namespace Odyssey.Epos.Systems
{
    public class BoundingBoxSystem : RunOnceSystem
    {
        public BoundingBoxSystem() : base(Selector.All(typeof(BoundingBoxComponent), typeof(TransformComponent)))
        {
        }

        public override void Process(ITimeService time)
        {
            foreach (var entity in Entities)
            {
                var cTransform = entity.GetComponent<TransformComponent>();
                var cBoundingBox = entity.GetComponent<BoundingBoxComponent>();

                var vMax = new Vector3(0.5f, 0.5f, 0.5f);
                var vMin = new Vector3(-0.5f, -0.5f, -0.5f);
                var bBox = new OrientedBoundingBox(vMin, vMax);
                bBox.Transform(cTransform.World);
                cBoundingBox.BoundingBox = bBox;
            }
        }
    }
}
