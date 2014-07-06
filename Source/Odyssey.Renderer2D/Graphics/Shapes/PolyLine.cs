using Odyssey.Engine;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.Graphics.Shapes
{
    public class PolyLine : PathGeometryBase
    {
        internal PolyLine(Direct2DDevice device)
            : base(device)
        {
            Initialize(Resource);
        }

        public static PolyLine New(Direct2DDevice device, IEnumerable<Vector2> points, FigureBegin figureBegin, FigureEnd figureEnd)
        {
            Contract.Requires<ArgumentNullException>(points != null);
            Contract.Requires<ArgumentException>(points.Count() >= 2, "Figure needs at least two points.");

            var array = points as Vector2[] ?? points.ToArray();
            PolyLine polyLine = new PolyLine(device);
            var sink = polyLine.DefineFigure();
            sink.BeginFigure(array[0], figureBegin);
            sink.AddLines(array.Skip(1));
            sink.CloseFigure(figureEnd);
            return polyLine;
        }
    }
}