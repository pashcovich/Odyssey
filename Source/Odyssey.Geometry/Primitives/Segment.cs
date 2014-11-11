using System;
using SharpDX.Mathematics;
using SharpDX.Mathematics;

namespace Odyssey.Geometry.Primitives
{
    public struct Segment : IEquatable<Segment>
    {
        private static readonly Vector3 tolerance = new Vector3(MathUtil.ZeroTolerance);
        public Vector3 A;
        public Vector3 B;

        public Segment(Vector3 a, Vector3 b)
        {
            A = a;
            B = b;
        }

        #region IEquatable

        public bool Equals(Segment other)
        {
            return Vector3.NearEqual(A, other.A, tolerance) && Vector3.NearEqual(B, other.B, tolerance);
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
