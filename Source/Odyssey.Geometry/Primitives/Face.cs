using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics;

namespace Odyssey.Geometry.Primitives
{
    public struct Face : IEquatable<Face>
    {
        public int Index1;
        public int Index2;
        public int Index3;

        public IEnumerable<UndirectedEdge> Edges
        {
            get
            {
                return new[]
                {
                    new UndirectedEdge(Index1, Index2),
                    new UndirectedEdge(Index2, Index3),
                    new UndirectedEdge(Index3, Index1),
                };
            }
        }

        public Face(int i1, int i2, int i3)
        {
            Index1 = i1;
            Index2 = i2;
            Index3 = i3;
        }


        public static Face[] Convert(int[] indices)
        {
            Contract.Requires<ArgumentNullException>(indices != null, "indices");
            Face[] faces = new Face[indices.Length/3];
            int faceIndex = 0;
            for (int i = 0; i < indices.Length; i+=3)
            {
                faces[faceIndex++] = new Face(indices[i], indices[i+1], indices[i+2]);
            }
            return faces;
        }

        public int RemainingIndex(UndirectedEdge edge)
        {
            Contract.Requires<ArgumentException>(edge.Item1 != edge.Item2, "edge");
            int i0 = edge.Item1;
            int i1 = edge.Item2;
            int[] indices = {Index1, Index2, Index3};

            int i2 = indices.DefaultIfEmpty(-1).Except(new[] {i0, i1}).FirstOrDefault();
            return i2;
        }

        #region IEquatable
        public bool Equals(Face other)
        {
            return Index2 == other.Index2 && Index1 == other.Index1 && Index3 == other.Index3;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Face && Equals((Face) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Index1;
                hashCode = (hashCode*397) ^ Index2;
                hashCode = (hashCode*397) ^ Index3;
                return hashCode;
            }
        }

        public static bool operator ==(Face left, Face right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Face left, Face right)
        {
            return !left.Equals(right);
        }

        #endregion

        public static bool FindSharedEdge(Face face1, Face face2, out UndirectedEdge edge)
        {
            Contract.Requires<ArgumentNullException>(face1.Edges != null, "face1");
            Contract.Requires<ArgumentNullException>(face2.Edges != null, "face2");

            foreach (var e1 in face1.Edges)
            {
                foreach (var e2 in face2.Edges)
                {
                    if (e1 == e2)
                    {
                        edge = e1;
                        return true;
                    }
                }
            }
            edge = default(UndirectedEdge);
            return false;
        }

        
    }
}
