using Odyssey.UserInterface.Style;
using SharpDX;

namespace Odyssey.Graphics.Drawing
{
    public interface IShape : IRenderable
    {
        RectangleF BoundingRectangle { get; }

        Brush Fill { get; }

        Brush Stroke { get; }

        float StrokeThickness { get; set; }
    }
}