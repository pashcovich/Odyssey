using SharpDX;

namespace Odyssey.Epos.Components
{
    [RequiredComponent(typeof(UIElementComponent))]
    public class BoundingBox2DComponent : Component
    {
        public RectangleF BoundingBox { get; set; }

        public BoundingBox2DComponent()
            : base(ComponentTypeManager.GetType<BoundingBox2DComponent>())
        {}
    }
}
