using Real = System.Single;
using Point = SharpDX.Vector2;

namespace Odyssey.Geometry.Primitives
{
    public interface IFigure
    {
        Real Width { get; }
        Real Height { get; }
        Point TopLeft { get; }
        FigureType FigureType { get; }
    }
}
