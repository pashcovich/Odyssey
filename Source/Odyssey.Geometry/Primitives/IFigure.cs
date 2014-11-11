using System;
using System.Collections.Generic;
using System.Linq;
using Real = System.Single;
using Point = SharpDX.Mathematics.Vector2;

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
