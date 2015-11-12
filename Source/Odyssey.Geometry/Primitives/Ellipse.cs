using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SharpDX;
using Real = System.Single;
using Point = SharpDX.Vector2;

namespace Odyssey.Geometry.Primitives
{
    public struct Ellipse : IEquatable<Ellipse>, IFigure
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

        public Ellipse(Real radiusX, Real radiusY) : this(Point.Zero, radiusX, radiusY)
        {
        }

        #region IEquatable<Ellipse>

        public bool Equals(Ellipse other)
        {
            return Center == other.Center && MathUtil.NearEqual(RadiusX, other.RadiusX) &&
                   MathUtil.NearEqual(RadiusY, other.RadiusY);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Ellipse && Equals((Ellipse) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Center.GetHashCode();
                hashCode = (hashCode*397) ^ RadiusX.GetHashCode();
                hashCode = (hashCode*397) ^ RadiusY.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Ellipse left, Ellipse right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Ellipse left, Ellipse right)
        {
            return !left.Equals(right);
        }

        #endregion

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
            var vertices = new Point[slices];
            const float radTo = MathHelper.TwoPi;
            float delta = radTo / slices;
            for (int i = 0; i < slices; i++)
            {
                Real theta = i * delta;
                vertices[i] = CreateEllipseVertex(center, radiusX, radiusY, theta);
            }
            return vertices;
        }

        public static IEnumerable<Point> CalculateArcVertices(Point center, Real radiusX, Real radiusY, float radFrom, float radTo, int slices)
        {
            Contract.Requires<ArgumentException>(radTo > radFrom, "'radTo' must be grether than 'radFrom'");
            var vertices = new Point[slices+1];
            float delta = (radTo-radFrom) / slices;

            for (int i = 0; i <= slices; i++)
            {
                Real theta = radFrom + i * delta;
                vertices[i] = CreateEllipseVertex(center, radiusX, radiusY, theta);
            }
            return vertices;
        }

        public static IEnumerable<Point> CalculateCircleVertices(Point center, Real radius, int slices)
        {
            return CalculateVertices(center, radius, radius, slices);
        }

            #region IFigure

        Point IFigure.TopLeft
        {
            get { return Center - new Vector2(RadiusX, RadiusY); }
        }

        Real IFigure.Width
        {
            get { return HorizontalAxis; }
        }

        Real IFigure.Height
        {
            get { return VerticalAxis; }
        }

        FigureType IFigure.FigureType
        {
            get { return FigureType.Ellipse; }
        }
        #endregion
    }
}
