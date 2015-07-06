using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Real = System.Single;
using Point = SharpDX.Mathematics.Vector2;

namespace Odyssey.Geometry.Primitives
{
    public struct Triangle : IPolygon
    {
        public Triangle(IEnumerable<Point> points) : this()
        {
            Contract.Requires<ArgumentNullException>(points != null, "points");
            Contract.Requires<ArgumentException>(points.Count()==3, "the array must have length 3");
            Vertices = new Vertices(points);
        }

        public Vertices Vertices { get; private set; }

        public Point Centroid
        {
            get { return Polygon.ComputeCentroid(Vertices); }
        }

        public Real Area
        {
            get { return Polygon.ComputeSignedArea(Vertices); }
        }

        public bool Contains(Point point)
        {
            return Polygon.PolygonPointTest(Vertices, point);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        public static Triangle NewIsosceles(Point topLeft, Real baseLength, Real height)
        {
            return new Triangle(new[]
            {
                new Point(topLeft.X + baseLength/2, topLeft.Y),
                new Point(topLeft.X, topLeft.Y + height),
                new Point(topLeft.X + baseLength, topLeft.Y + height)
            });
        }
    }
}
