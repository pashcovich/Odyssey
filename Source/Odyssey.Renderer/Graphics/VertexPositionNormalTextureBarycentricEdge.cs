using System;
using System.Runtime.InteropServices;
using Odyssey.Graphics.Meshes;
using SharpDX;

namespace Odyssey.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionNormalTextureBarycentricEdge : IEquatable<VertexPositionNormalTextureBarycentricEdge>
    {
        [VertexElement("SV_Position")] 
        public Vector3 Position;
        [VertexElement("NORMAL")] 
        public Vector3 Normal;

        /// <summary>
        /// Gets or sets the texture coordinate for the vertex.
        /// </summary>
        [VertexElement("TEXCOORD0")]
        public Vector2 TextureUV;

        [VertexElement("BARYCENTRIC")]
        public Vector3 Barycentric;

        [VertexElement("EDGE")]
        public float ExcludeEdge;

        public const int Stride = 48;

        public VertexPositionNormalTextureBarycentricEdge(Vector3 position, Vector3 normal, Vector2 textureUV, Vector3 barycentric, float excludeEdge)
            : this()
        {
            Position = position;
            TextureUV = textureUV;
            Normal = normal;
            Barycentric = barycentric;
            ExcludeEdge = excludeEdge;
        }

        #region Equality members

        public bool Equals(VertexPositionNormalTextureBarycentricEdge other)
        {
            return Position.Equals(other.Position) && Normal.Equals(other.Normal) && TextureUV.Equals(other.TextureUV) 
                && Barycentric.Equals(other.Barycentric) && ExcludeEdge.Equals(other.ExcludeEdge);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexPositionNormalTextureBarycentricEdge && Equals((VertexPositionNormalTextureBarycentricEdge)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Position.GetHashCode();
                hashCode = (hashCode*397) ^ Normal.GetHashCode();
                hashCode = (hashCode*397) ^ TextureUV.GetHashCode();
                hashCode = (hashCode * 397) ^ Barycentric.GetHashCode();
                hashCode = (hashCode * 397) ^ ExcludeEdge.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VertexPositionNormalTextureBarycentricEdge left, VertexPositionNormalTextureBarycentricEdge right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionNormalTextureBarycentricEdge left, VertexPositionNormalTextureBarycentricEdge right)
        {
            return !left.Equals(right);
        }

        #endregion Equality members

    }
}