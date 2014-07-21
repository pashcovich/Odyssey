using System;
using System.Collections.Generic;
using Real = System.Single;
using Point = SharpDX.Vector2;

namespace Odyssey.Geometry.Primitives
{
    public struct Ellipse : IEquatable<Ellipse>
    {
        public readonly Point Center;
        public readonly Real RadiusX;
        public readonly Real RadiusY;

        public Ellipse(Point center, Real radiusX, Real radiusY)
        {
            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
        }

        public bool Equals(Ellipse other)
        {
            return (Center == other.Center) &&
                   (Math.Abs(RadiusX - other.RadiusX) < MathHelper.EpsilonD) &&
                    (Math.Abs(RadiusY - other.RadiusY) < MathHelper.EpsilonD);
        }

        public Real HorizontalAxis
        {
            get { return 2*RadiusX; }
        }

        public Real VerticalAxis
        {
            get { return 2*RadiusY; }
        }

        public IEnumerable<Point> CalculateVertices(int slices)
        {
            return CalculateVertices(Center, RadiusX, RadiusY, slices);
        }

        internal static Point CreateEllipseVertex(Point center, Real radiusX, Real radiusY, Real theta, Real offset = 1)
        {
            return new Point(center.X + (Real)Math.Cos(theta) * radiusX * offset, center.Y + (Real)Math.Sin(theta) * radiusY * offset);
        }

        public static IEnumerable<Point> CalculateVertices(Point center, Real radiusX, Real radiusY, int slices)
        {
            Point[] vertices = new Point[slices];
            const float radTo = MathHelper.TwoPi;
            float delta = radTo / slices;
            for (int i = 0; i < slices; i++)
            {
                Real theta = i * delta;
                vertices[i] = CreateEllipseVertex(center, radiusX, radiusY, theta);
            }
            return vertices;
        }
         
    }
}
