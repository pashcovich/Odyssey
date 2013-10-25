using Odyssey.Engine;
using Odyssey.UserInterface.Style;
using SharpDX;

namespace Odyssey.Graphics.Rendering2D.Shapes
{
    public interface IShape : IRenderable
    {
        RectangleF BoundingRectangle { get; set; }
        IGradient Fill { get; set; }
        IGradient Stroke { get; set; }
    }
}