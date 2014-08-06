using Odyssey.UserInterface.Style;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public interface IShape : IRenderable
    {
        RectangleF BoundingRectangle { get; }

        string FillGradientClass { get; set; }

        string StrokeGradientClass { get; set; }

        float StrokeThickness { get; set; }
    }
}