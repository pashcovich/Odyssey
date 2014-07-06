using Odyssey.Engine;
using Odyssey.Geometry.Primitives;
using SharpDX;
using System.Linq;

namespace Odyssey.Graphics.Shapes
{
    public class PolygonGeometry : PathGeometryBase
    {
        private readonly Polygon polygon;
        private readonly int sides;

        public Vector2 Center
        {
            get { return polygon.Centroid; }
        }

        public int Sides
        {
            get { return polygon.Count; }
        }

        internal PolygonGeometry(Direct2DDevice device, Polygon polygon)
            : base(device)
        {
            this.polygon = polygon;
            Initialize(Resource);
        }

        /// <summary>
        /// <see cref="Polygon"/> casting operator.
        /// </summary>
        /// <param name="from">From the PathGeometry.</param>
        public static implicit operator Polygon(PolygonGeometry from)
        {
            return from == null ? null : from.polygon ?? null;
        }

        public static PolygonGeometry New(Direct2DDevice device, Vector2 center, float circumCircleRadius, int sides, FigureBegin figureBegin)
        {
            var polygon = Polygon.New(center, circumCircleRadius, sides);

            PolygonGeometry polygonGeometry = new PolygonGeometry(device, polygon);
            var sink = polygonGeometry.DefineFigure();
            sink.BeginFigure(polygon[0], figureBegin);
            sink.AddLines(polygon.Skip(1));
            sink.CloseFigure(FigureEnd.Closed);
            return polygonGeometry;
        }
    }
}