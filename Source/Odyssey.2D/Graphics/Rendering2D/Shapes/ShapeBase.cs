using Odyssey.Engine;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Style;
using SharpDX;
using System.Collections.Generic;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public abstract class ShapeBase : UIElement, IShape
    {
        public ShapeBase()
        { }

        public IGradient FillShader { get; set; }
        RectangleF IShape.BoundingRectangle
        {
            get { return BoundingRectangle; }
            set { BoundingRectangle = value; }
        }
        public IGradient StrokeShader { get; set; }

        public abstract void Render(IDirectXTarget target);

        public abstract void UpdateExtents(RectangleF polygon);
       
    }
}