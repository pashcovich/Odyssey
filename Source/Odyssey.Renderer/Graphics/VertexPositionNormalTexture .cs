using System;
using System.Runtime.InteropServices;
using Odyssey.Graphics.Meshes;
using SharpDX;

namespace Odyssey.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionNormalTexture : IEquatable<VertexPositionNormalTexture>
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

        public const int Stride = 32;

        public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 textureUV)
            : this()
        {
            Position = position;
            TextureUV = textureUV;
            Normal = normal;
        }

        #region Equality members

        public bool Equals(VertexPositionNormalTexture other)
        {
            return Position.Equals(other.Position) && Normal.Equals(other.Normal) && TextureUV.Equals(other.TextureUV);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexPositionNormalTexture && Equals((VertexPositionNormalTexture) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Position.GetHashCode();
                hashCode = (hashCode*397) ^ Normal.GetHashCode();
                hashCode = (hashCode*397) ^ TextureUV.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
        {
            return !left.Equals(right);
        }

        #endregion Equality members

    }
}