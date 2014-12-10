#region Using Directives

using System;
using Odyssey.Animations;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.Graphics
{
    public class GradientStop : IEquatable<GradientStop>
    {
        public GradientStop(Color4 color, float offset)
            : this()
        {
            Color = color;
            Offset = offset;
        }

        public GradientStop()
        {
            Offset = -1;
        }

        #region Equality

        public bool Equals(GradientStop other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Color.Equals(Color) && other.Offset.Equals(Offset);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (GradientStop)) return false;
            return Equals((GradientStop) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Color.GetHashCode()*397) ^ Offset.GetHashCode();
            }
        }

        public static bool operator ==(GradientStop left, GradientStop right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GradientStop left, GradientStop right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("Offset: {0:0.00} Color: {1}", Offset, Color);
        }

        #endregion Equality

        internal int Index { get; set; }

        [Animatable]
        public Color4 Color { get; set; }

        public float Offset { get; set; }

        public static explicit operator SharpDX.Direct2D1.GradientStop(GradientStop from)
        {
            return from == null ? default(SharpDX.Direct2D1.GradientStop) : new SharpDX.Direct2D1.GradientStop {Color = from.Color, Position = from.Offset};
        }
    }
}