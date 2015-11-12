using System;
using System.Runtime.InteropServices;
using SharpDX;

namespace Odyssey.Graphics
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionColor : IEquatable<VertexPositionColor>
    {
        [VertexElement("SV_Position")] 
        public Vector3 Position;
        [VertexElement("COLOR")] 
        public Vector4 Color;

        public VertexPositionColor(Vector3 position, Vector4 color)
            : this()
        {
            Position = position;
            Color= color;
        }

        #region Equality members

        public bool Equals(VertexPositionColor other)
        {
            return Position.Equals(other.Position) && Color.Equals(other.Color);
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
                hashCode = (hashCode*397) ^ Color.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(VertexPositionColor left, VertexPositionColor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionColor left, VertexPositionColor right)
        {
            return !left.Equals(right);
        }

        #endregion Equality members

    }
}