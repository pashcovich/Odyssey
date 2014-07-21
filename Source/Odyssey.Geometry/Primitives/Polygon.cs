using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharpDX;
using Real = System.Single;
using Point = SharpDX.Vector2;

namespace Odyssey.Geometry.Primitives
{
    public class Polygon : IPolygon
    {
        #region Properties

        public Vertices Vertices { get; private set; }

        public Point Centroid
        {
            get { return ComputeCentroid(this); }
        }

        public Real Area
        {
            get { return Math.Abs(ComputeSignedArea(this)); }
        }

        public int Count
        {
            get { return Vertices.Count; }
        }

        public Matrix3x2 Transform { get; set; }

        public bool IsCounterClockWise
        {
            get
            {
                //We just return true for lines
                if (Vertices.Count < 3)
                    return true;

                return (ComputeSignedArea(this) > 0.0);
            }
        }

        public Point this[int index]
        {
            get { return Vertices[index]; }
        }

        #endregion

        #region Constructors

        internal Polygon(IEnumerable<Point> points)
            : this()
        {
            Vertices.AddRange(points);
        }

        internal Polygon()
        {
            Vertices = new Vertices();
        }

        #endregion

        /// <summary>
        /// Forces counter clock wise order.
        /// </summary>
        public void ForceCounterClockWise()
        {
            if (!IsCounterClockWise)
                Vertices.Reverse();
        }

        /// <summary>
        /// Translates the vertices using the specified offset.
        /// </summary>
        /// <param name="xOffset">Horizontal offset.</param>
        /// <param name="yOffset">Vertical offset.</param>
        public void Translate(Real xOffset, Real yOffset)
        {
            Transform = Matrix3x2.Translation(xOffset, yOffset);
        }

        /// <summary>
        /// ranslates the vertices using the specified vector.
        /// </summary>
        /// <param name="vector">Translation vector.</param>
        public void Translate(Point vector)
        {
            Transform = Matrix3x2.Translation(vector);
        }

        public bool Contains(Point point)
        {
            Matrix3x2 inverse = Matrix3x2.Invert(Transform);
            Point p = Matrix3x2.TransformPoint(inverse, point);
            return PolygonPointTest(Vertices, p);
        }

        #region SceneStatic methods

        public static Point ComputeCentroid(Polygon polygon)
        {
            Point centroid = new Point(0, 0);
            Real signedArea = 0.0f;
            Real x0; // Current vertex X
            Real y0; // Current vertex Y
            Real x1; // Next vertex X
            Real y1; // Next vertex Y
            Real a; // Partial signed area
            Point[] vertices = polygon.Vertices.ToArray();

            // For all vertices except last
            int i;
            for (i = 0; i < polygon.Vertices.Count - 1; ++i)
            {
                x0 = vertices[i].X;
                y0 = vertices[i].Y;
                x1 = vertices[i + 1].X;
                y1 = vertices[i + 1].Y;
                a = x0*y1 - x1*y0;
                signedArea += a;
                centroid.X += (x0 + x1)*a;
                centroid.Y += (y0 + y1)*a;
            }

            // Do last vertex
            x0 = vertices[i].X;
            y0 = vertices[i].Y;
            x1 = vertices[0].X;
            y1 = vertices[0].Y;
            a = x0*y1 - x1*y0;
            signedArea += a;
            centroid.X += (x0 + x1)*a;
            centroid.Y += (y0 + y1)*a;

            signedArea /= 2;
            centroid.X /= (6*signedArea);
            centroid.Y /= (6*signedArea);

            return centroid;
        }

        public static bool ConvexityTest(Polygon polygon)
        {
            if (polygon.Vertices.Count < 3) return false;

            int i;
            int n = polygon.Vertices.Count;
            int flag = 0;
            Vertices p = polygon.Vertices;

            for (i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                int k = (i + 2) % n;
                Real z = (p[j].X - p[i].X) * (p[k].Y - p[j].Y);
                z -= (p[j].Y - p[i].Y) * (p[k].X - p[j].X);
                if (z < 0)
                    flag |= 1;
                else if (z > 0)
                    flag |= 2;
                if (flag == 3)
                    return false;
            }
            return flag != 0;
        }

        #endregion

        //public static explicit operator PathFigure(Polygon polygon)
        //{
        //    List<Segment> segments = new List<Segment>();
        //    Point s = polygon.Vertices[polygon.Vertices.Count - 1];
        //    for (int i = 0; i < polygon.Vertices.Count; i++)
        //    {
        //        Point p = polygon.Vertices[i];
        //        segments.Add(new Segment(s, p));

        //        s = p;
        //    }

        //    return new PathFigure(segments);
        //}

        /// <summary>
        /// Returns an AABB for vertex.
        /// </summary>
        /// <returns></returns>
        //public AABB2D GetCollisionBox()
        //{
        //    AABB2D aabb;
        //    Point lowerBound = new Point(Real.MaxValue, Real.MaxValue);
        //    Point upperBound = new Point(Real.MinValue, Real.MinValue);

        //    for (int i = 0; i < Vertices.Count; ++i)
        //    {
        //        if (Vertices[i].X < lowerBound.X)
        //            lowerBound.X = Vertices[i].X;
        //        if (Vertices[i].X > upperBound.X)
        //            upperBound.X = Vertices[i].X;
        //        if (Vertices[i].Y < lowerBound.Y)
        //            lowerBound.Y = Vertices[i].Y;
        //        if (Vertices[i].Y > upperBound.Y)
        //            upperBound.Y = Vertices[i].Y;
        //    }

        //    aabb.LowerBound = lowerBound;
        //    aabb.UpperBound = upperBound;

        //    return aabb;
        //}

        public Polygon Copy()
        {
            return new Polygon(Vertices);
        }

        #region IEnumerator<Point>

        public IEnumerator<Point> GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Vertices.GetEnumerator();
        }

        #endregion


        public static bool PolygonPointTest(Vertices vertices, Point point)
        {
            // Crossing Test
            // Source: Real Time Rendering 3rd Edition, p. 754

            bool inside = false;
            Point t = point;
            Point e0 = vertices[vertices.Count - 1];
            bool y0 = e0.Y >= t.Y;

            for (int i = 0; i < vertices.Count - 1; i++)
            {
                Point e1 = vertices[i];
                bool y1 = e1.Y >= t.Y;
                if (y0 != y1)
                {
                    if (((e1.Y - t.Y) * (e0.X - e1.X) >= (e1.X - t.X) * (e0.Y - e1.Y)) == y1)
                        inside = !inside;
                }
                y0 = y1;
                e0 = e1;
            }
            return inside;
        }

        public static Real ComputeSignedArea(Polygon polygon)
        {
            int i;
            Real area = 0;

            Vertices vertices = polygon.Vertices;

            for (i = 0; i < vertices.Count; i++)
            {
                int j = (i + 1) % vertices.Count;
                area += vertices[i].X * vertices[j].Y;
                area -= vertices[i].Y * vertices[j].X;
            }
            area /= 2;
            return area;

        }

        /// <summary>
        /// Winding number test for a point in a polygon.
        /// </summary>
        /// See more info about the algorithm here: http://softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm
        /// <param name="polygon">The polygon.</param>
        /// <param name="point">The point to be tested.</param>
        /// <returns>-1 if the winding number is zero and the point is outside
        /// the polygon, 1 if the point is inside the polygon, and 0 if the point
        /// is on the polygons edge.</returns>
        public static int PointInPolygon(Polygon polygon, Point point)
        {
            // Winding number
            int wn = 0;

            Vertices polyVertices = polygon.Vertices;
            // Iterate through polygon's edges
            for (int i = 0; i < polyVertices.Count; i++)
            {
                // Get points
                Point p1 = polyVertices[i];
                Point p2 = polyVertices[polyVertices.NextIndex(i)];

                // Test if a point is directly on the edge
                Point edge = p2 - p1;
                double area = GeometryHelper.Area(ref p1, ref p2, ref point);
                if (Math.Abs(area - 0f) < MathHelper.EpsilonD && Point.Dot(point - p1, edge) >= 0 && Point.Dot(point - p2, edge) <= 0)
                {
                    return 0;
                }
                // Test edge for intersection with ray from point
                if (p1.Y <= point.Y)
                {
                    if (p2.Y > point.Y && area > 0)
                    {
                        ++wn;
                    }
                }
                else
                {
                    if (p2.Y <= point.Y && area < 0)
                    {
                        --wn;
                    }
                }
            }
            return wn;
        }

        public static Polygon New(Point center, Real circumCircleRadius, int sides)
        {
            float polygonSide = SideLength(circumCircleRadius, sides);

            Vector2[] polygonPoints = new Vector2[sides];
            
            for (int i = 0; i < sides; i++)
            {
                float theta = 2 * MathUtil.Pi / sides;
                float x = center.X + (float)Math.Cos(i * theta) * polygonSide;
                float y = center.Y + (float)Math.Sin(i * theta) * polygonSide;

                polygonPoints[i] = new Vector2(x, y);
            }

            return new Polygon(polygonPoints);
        }

        static float SideLength(float circumCircleRadius, int sides)
        {
            return 2*circumCircleRadius*(float)Math.Sin(Math.PI/sides);
        }
    }
}