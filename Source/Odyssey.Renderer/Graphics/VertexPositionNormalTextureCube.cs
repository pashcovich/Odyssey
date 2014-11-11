using System;
using System.Runtime.InteropServices;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Meshes
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionNormalTextureCube :IEquatable<VertexPositionNormalTexture>
    {
        [VertexElement("SV_Position")]
        public Vector3 Position;
        [VertexElement("NORMAL")]
        public Vector3 Normal;

        /// <summary>
        /// Gets or sets the texture coordinate for the vertex.
        /// </summary>
        [VertexElement("TEXCOORD0")]
        public Vector3 TextureUVW;

        public const int Stride = 36;


        public VertexPositionNormalTextureCube(Vector3 position, Vector3 normal, Vector3 textureUVW)
            : this()
        {
            Position = position;
            TextureUVW = textureUVW;
            Normal = normal;
        }

        #region Equality members

        public bool Equals(VertexPositionNormalTexture other)
        {
            return Position.Equals(other.Position) && Normal.Equals(other.Normal) && TextureUVW.Equals(other.TextureUV);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexPositionNormalTexture && Equals((VertexPositionNormalTexture)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.GetHashCode();
                hashCode = (hashCode * 397) ^ TextureUVW.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VertexPositionNormalTextureCube left, VertexPositionNormalTextureCube right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionNormalTextureCube left, VertexPositionNormalTextureCube right)
        {
            return !left.Equals(right);
        }

        #endregion Equality members

    }
}