using SharpDX;

namespace Odyssey.Graphics
{
    public abstract class RectangleBase : ShapeBase
    {
        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

    }
}
