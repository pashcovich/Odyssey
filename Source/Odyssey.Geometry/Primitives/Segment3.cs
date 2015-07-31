using System;
using SharpDX.Mathematics;
using Real = System.Single;
using Point3 = SharpDX.Mathematics.Vector3;

namespace Odyssey.Geometry.Primitives
{
    public struct Segment3 : IEquatable<Segment3>
    {
        private static readonly Vector3 tolerance = new Point3(MathUtil.ZeroTolerance);
        public Point3 A;
        public Point3 B;

        public Segment3(Point3 a, Point3 b)
        {
            A = a;
            B = b;
        }

        //http://math.stackexchange.com/questions/322831/determing-the-distance-from-a-line-segment-to-a-point-in-3-space

        public Real Length
        {
            get { return (B - A).Length(); }
        }

        public Real LengthSquared
        {
            get { return (B - A).LengthSquared(); }
        }

        public static float Distance(Segment3 s1, Segment3 s2)
        {
            Vector3 u = s1.B - s1.A;
            Vector3 v = s2.B - s2.A;
            Vector3 w = s1.A - s2.A;
            Real a = Vector3.Dot(u, u);         // always >= 0
            Real b = Vector3.Dot(u, v);
            Real c = Vector3.Dot(v, v);         // always >= 0
            Real d = Vector3.Dot(u, w);
            Real e = Vector3.Dot(v, w);
            Real D = a * c - b * b;        // always >= 0
            Real sc, sN, sD = D;       // sc = sN / sD, default sD = D >= 0
            Real tc, tN, tD = D;       // tc = tN / tD, default tD = D >= 0

            // compute the line parameters of the two closest points
            bool parallel = b / (u.Length() * v.Length()) > 1 - MathHelper.Epsilon;
            if (parallel)
            { // the lines are almost parallel
                sN = 0.0f;         // force using point P0 on segment S1
                sD = 1.0f;         // to prevent possible division by 0.0 later
                tN = e;
                tD = c;
            }
            else
            {                 // get the closest points on the infinite lines
                sN = (b * e - c * d);
                tN = (a * e - b * d);
                if (sN < 0.0)
                {        // sc < 0 => the s=0 edge is visible
                    sN = 0.0f;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD)
                {  // sc > 1  => the s=1 edge is visible
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }

            if (tN < 0.0f)
            {            // tc < 0 => the t=0 edge is visible
                tN = 0.0f;
                // recompute sc for this edge
                if (-d < 0.0f)
                    sN = 0.0f;
                else if (-d > a)
                    sN = sD;
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD)
            {      // tc > 1  => the t=1 edge is visible
                tN = tD;
                // recompute sc for this edge
                if ((-d + b) < 0.0)
                    sN = 0;
                else if ((-d + b) > a)
                    sN = sD;
                else
                {
                    sN = (-d + b);
                    sD = a;
                }
            }
            // finally do the division to get sc and tc
            sc = (Math.Abs(sN) < MathHelper.Epsilon ? 0.0f : sN / sD);
            tc = (Math.Abs(sN) < MathHelper.Epsilon ? 0.0f : tN / tD);

            // get the difference of the two closest points
            Vector3 dP = w + (sc * u) - (tc * v);  // =  S1(sc) - S2(tc)

            return dP.Length();   // return the closest distance
        }


        #region IEquatable

        public bool Equals(Segment3 other)
        {
            return Point3.NearEqual(A, other.A, tolerance) && Point3.NearEqual(B, other.B, tolerance);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Segment3 && Equals((Segment3) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (A.GetHashCode()*397) ^ B.GetHashCode();
            }
        }

        public static bool operator ==(Segment3 left, Segment3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Segment3 left, Segment3 right)
        {
            return !left.Equals(right);
        }

        #endregion

        public static implicit operator Line3(Segment3 s)
        {
            return new Line3(s.A, Vector3.Normalize(s.B - s.A));
        }

    }
}
