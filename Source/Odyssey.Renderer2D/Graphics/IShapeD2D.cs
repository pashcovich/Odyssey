using SharpDX.Direct2D1;
using Brush = Odyssey.Graphics.Shapes.Brush;

namespace Odyssey.Graphics
{
    public interface IControlD2D
    {
        Brush BorderBrush { get; }
        Brush Foreground { get; }
    }

    public interface IShapeD2D : IShape
    {
        Brush Fill { get; set; }
        Brush Stroke { get; set; }
    }
}