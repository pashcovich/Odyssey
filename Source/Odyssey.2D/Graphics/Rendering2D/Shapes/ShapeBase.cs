using Odyssey.Engine;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Style;
using SharpDX;
using System.Collections.Generic;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public abstract class ShapeBase : UIElement, IShape
    {
        public ShapeBase(string id)
            : base(id)
        { }

        public IGradient Fill { get; set; }
        RectangleF IShape.BoundingRectangle
        {
            get { return BoundingRectangle; }
            set { BoundingRectangle = value; }
        }
        public IGradient Stroke { get; set; }

        public abstract void Initialize(IDirectXProvider directX);

        public abstract void Render(IDirectXTarget target);

        public abstract void UpdateExtents(RectangleF polygon);
    }
}