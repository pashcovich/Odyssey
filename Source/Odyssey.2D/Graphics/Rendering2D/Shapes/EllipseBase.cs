using Odyssey.UserInterface;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public abstract class EllipseBase : ShapeBase
    {
        protected EllipseBase() : base() { }
        
        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        public override void UpdateExtents(RectangleF rectangle)
        {
            AbsolutePosition = new Vector2((float)rectangle.X, (float)rectangle.Y);
            Width = (float)rectangle.Width;
            Height = (float)rectangle.Height;
            BoundingRectangle = rectangle;
        }
    }
}
