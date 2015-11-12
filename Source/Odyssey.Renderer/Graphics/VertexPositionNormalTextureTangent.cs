using System;
using System.Runtime.InteropServices;
using SharpDX;

namespace Odyssey.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionNormalTextureTangent :IEquatable<VertexPositionNormalTextureTangent>
    {
        public const int Stride = 48;
        
        [VertexElement("SV_Position")] 
        public Vector3 Position;
        [VertexElement("NORMAL")] 
        public Vector3 Normal;

        /// <summary>
        /// Gets or sets the texture coordinate for the vertex.
        /// </summary>
        [VertexElement("TEXCOORD0")]
        public Vector2 TextureUV;

        [VertexElement("TANGENT")] public Vector4 Tangent;
        
        public VertexPositionNormalTextureTangent(Vector3 position, Vector3 normal, Vector2 textureUV, Vector4 tangent)
            : this()
        {
            Position = position;
            TextureUV = textureUV;
            Normal = normal;
            Tangent = tangent;
        }

        #region Equality members

        public bool Equals(VertexPositionNormalTextureTangent other)
        {
            return Position.Equals(other.Position) && Normal.Equals(other.Normal) && TextureUV.Equals(other.TextureUV) && Tangent.Equals(other.Tangent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexPositionNormalTextureTangent && Equals((VertexPositionNormalTextureTangent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Position.GetHashCode();
                hashCode = (hashCode*397) ^ Normal.GetHashCode();
                hashCode = (hashCode*397) ^ TextureUV.GetHashCode();
                hashCode = (hashCode * 397) ^ Tangent.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VertexPositionNormalTextureTangent left, VertexPositionNormalTextureTangent right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionNormalTextureTangent left, VertexPositionNormalTextureTangent right)
        {
            return !left.Equals(right);
        }

        #endregion Equality members

    }
}