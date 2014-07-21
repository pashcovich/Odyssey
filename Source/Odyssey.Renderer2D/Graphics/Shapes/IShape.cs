using Odyssey.UserInterface.Style;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public interface IShape : IRenderable
    {
        RectangleF BoundingRectangle { get; }

        IGradient FillShader { get; set; }

        IGradient StrokeShader { get; set; }
    }
}