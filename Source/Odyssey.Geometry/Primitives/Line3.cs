using System;
using Odyssey.Geometry.Extensions;
using SharpDX;
using Real = System.Single;
using Point3 = SharpDX.Vector3;

namespace Odyssey.Geometry.Primitives
{
    public struct Line3 : IEquatable<Line3>
    {
        public Point3 Origin { get; private set; }
        public Point3 Direction { get; private set; }

        public Line3(Point3 origin, Point3 direction)
            : this()
        {
            Origin = origin;
            Direction = direction;
        }

        public Point3 PointAtDistance(Real distance)
        {
            return Origin + distance*Point3.Normalize(Direction);
        }

        //D = ||PQ x u|| / ||u||

        public Real Distance(Point3 p)
        {
            var num = Point3.Cross((p - Origin), Direction).Length();
            var denom = Direction.Length();
            return num/denom;
        }

        // dist3D_Line_to_Line(): get the 3D minimum distance between 2 lines
        //    Input:  two 3D lines L1 and L2
        //    Return: the shortest distance between L1 and L2
        public static Real DistanceLineLine(Line3 l1, Line3 l2)
        {
            var u = l1.Direction;
            var v = l2.Direction;
            var w = l1.Origin - l2.Origin;

            Real a = Point3.Dot(u, u);
            Real b = Point3.Dot(u, v);
            Real c = Point3.Dot(v, v);
            Real d = Point3.Dot(u, w);
            Real e = Point3.Dot(v, w);
            Real distance = a*c - b*b;

            Real sc, tc;
            bool parallel = b / (u.Length() * v.Length()) > 1 - MathHelper.Epsilon;
            if (parallel)
            {
                sc = 0;
                tc = (b > c ? d / b : e / c);
            }
            else
            {
                sc = (b * e - c * d) / distance;
                tc = (a * e - b * d) / distance;
            }

            var dP = w + (sc*u) - (tc*v);
            return dP.Length();
        }

        public static explicit operator Line3(Ray r)
        {
            return new Line3(r.Position, Vector3.Normalize(r.Direction));
        }

         #region Equality

        public bool Equals(Line3 other)
        {
            return other.Origin.Equals(Origin) && other.Direction.Equals(Direction);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Line3)) return false;
            return Equals((Line3) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Origin.GetHashCode()*397) ^ Direction.GetHashCode();
            }
        }

        public static bool operator ==(Line3 left, Line3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Line3 left, Line3 right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}