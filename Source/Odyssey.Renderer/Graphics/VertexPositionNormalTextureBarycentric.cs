using System;
using System.Runtime.InteropServices;
using Odyssey.Graphics.Meshes;
using SharpDX.Mathematics;

namespace Odyssey.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionNormalTextureBarycentric : IEquatable<VertexPositionNormalTextureBarycentric>
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

        public const int Stride = 44;

        public VertexPositionNormalTextureBarycentric(Vector3 position, Vector3 normal, Vector2 textureUV, Vector3 barycentric)
            : this()
        {
            Position = position;
            TextureUV = textureUV;
            Normal = normal;
            Barycentric = barycentric;
        }

        #region Equality members

        public bool Equals(VertexPositionNormalTextureBarycentric other)
        {
            return Position.Equals(other.Position) && Normal.Equals(other.Normal) && TextureUV.Equals(other.TextureUV) && Barycentric.Equals(other.Barycentric);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VertexPositionNormalTextureBarycentric && Equals((VertexPositionNormalTextureBarycentric)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Position.GetHashCode();
                hashCode = (hashCode*397) ^ Normal.GetHashCode();
                hashCode = (hashCode*397) ^ TextureUV.GetHashCode();
                hashCode = (hashCode * 397) ^ Barycentric.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VertexPositionNormalTextureBarycentric left, VertexPositionNormalTextureBarycentric right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionNormalTextureBarycentric left, VertexPositionNormalTextureBarycentric right)
        {
            return !left.Equals(right);
        }

        #endregion Equality members

    }
}