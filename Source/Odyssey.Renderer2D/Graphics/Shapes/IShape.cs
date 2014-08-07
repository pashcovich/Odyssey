using Odyssey.UserInterface.Style;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public interface IShape : IRenderable
    {
        RectangleF BoundingRectangle { get; }

        Gradient FillGradient { get; set; }

        Gradient StrokeGradient { get; set; }

        float StrokeThickness { get; set; }
    }
}