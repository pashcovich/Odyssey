using System;
using SharpDX;
using Real = System.Single;
using Point = SharpDX.Vector2;

namespace Odyssey.Geometry.Primitives
{
    public struct Segment : IEquatable<Segment>
    {
        private static readonly Point tolerance = new Point(MathUtil.ZeroTolerance);
        public Point A;
        public Point B;

        public Segment(Point a, Point b)
        {
            A = a;
            B = b;
        }

        //http://math.stackexchange.com/questions/322831/determing-the-distance-from-a-line-segment-to-a-point-in-3-space

        public Real Length
        {
            get { return (B - A).Length(); }
        }

        public Vector2 Direction
        {
            get { return B - A; }
        }

        public Real LengthSquared
        {
            get { return (B - A).LengthSquared(); }
        }

        public static Real Distance(Segment s, Point p)
        {
            var l2 = s.LengthSquared;
            if (MathHelper.IsCloseToZero(l2))
                return Point.Distance(s.A, p);
            var t = Vector2.Dot(p - s.A, s.Direction)/s.LengthSquared;
            if (t < 0) return Point.Distance(p, s.A);
            if (t > 1) return Point.Distance(p, s.B);
            var projection = s.A + t*s.Direction;
            return Point.Distance(p, projection);
        }



        #region IEquatable

        public bool Equals(Segment other)
        {
            return (A == other.A) && (B == other.B);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Segment && Equals((Segment) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (A.GetHashCode()*397) ^ B.GetHashCode();
            }
        }

        public static bool operator ==(Segment left, Segment right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Segment left, Segment right)
        {
            return !left.Equals(right);
        }

        #endregion

    }
}
