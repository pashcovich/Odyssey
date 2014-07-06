using SharpDX;

namespace Odyssey.Graphics
{
    public abstract class CutCornerRectangleBase : ShapeBase
    {
        const float DefaultCornerLength = 16;

        public float CutCornerLength { get; set; }

        protected CutCornerRectangleBase() : base() {
            CutCornerLength = DefaultCornerLength;
        }

        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

    }
}
