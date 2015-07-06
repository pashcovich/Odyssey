using SharpDX.Mathematics;

namespace Odyssey.Epos.Components
{
    public class BoundingBoxComponent : Component
    {
        public OrientedBoundingBox BoundingBox { get; set; }

        public BoundingBoxComponent() : base(ComponentTypeManager.GetType<BoundingBoxComponent>())
        {}
    }
}
