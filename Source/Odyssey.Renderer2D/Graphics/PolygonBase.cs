#region Using Directives

using Odyssey.Geometry.Primitives;
using SharpDX;

#endregion Using Directives

namespace Odyssey.Graphics
{
    public abstract class PolygonBase : ShapeBase
    {
        protected abstract Polygon Polygon { get; set; }

        public override bool Contains(Vector2 cursorLocation)
        {
            return Polygon.Contains(cursorLocation);
        }
    }
}