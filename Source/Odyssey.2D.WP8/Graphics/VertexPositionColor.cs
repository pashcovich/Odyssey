#if !ODYSSEY_ENGINE
using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    /// <summary>
    /// Represents a vertex with a position and a color vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VertexPositionColor : IEquatable<VertexPositionColor>
    {
        private static readonly InputElement[] inputElements;
        public const int Stride = 32;

        static VertexPositionColor()
        {
            inputElements = new[]
                                {
                                    new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
                                    new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0)
                                };
        }
        /// <summary>
        /// Gets or sets the position of the vertex.
        /// </summary>
        public Vector4 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the texture coordinate for the vertex.
        /// </summary>
        public Color4 Color
        {
            get;
            set;
        }

        public static InputElement[] InputElements
        {
            get { return inputElements; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionColor"/> struct.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        public VertexPositionColor(Vector4 position, Color4 color)
            : this()
        {
            Position = position;
            Color = color;
        }

        /// <summary>
        /// Implements operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(VertexPositionColor left, VertexPositionColor right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements operator !=.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(VertexPositionColor left, VertexPositionColor right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return Position.GetHashCode() + Color.GetHashCode();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            return Equals((VertexPositionColor)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(VertexPositionColor other)
        {
            return (Position == other.Position && Color == other.Color);
        }


    }
}
#endif