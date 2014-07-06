using System;

namespace Odyssey.Geometry.Primitives
{
    public struct UndirectedEdge : IEquatable<UndirectedEdge>
    {
        public UndirectedEdge(int item1, int item2)
        {
            // Makes an undirected edge. Rather than overloading comparison operators to give us the (a,b)==(b,a) property,
            // we'll just ensure that the larger of the two goes first. This'll simplify things greatly.
            Item1 = Math.Max(item1, item2);
            Item2 = Math.Min(item1, item2);
        }

        public readonly int Item1;

        public readonly int Item2;

        public bool Equals(UndirectedEdge other)
        {
            return Item1 == other.Item1 && Item2 == other.Item2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UndirectedEdge && Equals((UndirectedEdge)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Item1.GetHashCode() * 397) ^ Item2.GetHashCode();
            }
        }

        public static bool operator ==(UndirectedEdge left, UndirectedEdge right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UndirectedEdge left, UndirectedEdge right)
        {
            return !left.Equals(right);
        }
    }
}