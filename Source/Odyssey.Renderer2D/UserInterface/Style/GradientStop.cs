using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Style
{
    public class GradientStop : IEquatable<GradientStop>
    {
        public Color4 Color { get; set; }

        public float Offset { get; set; }

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
            if (obj.GetType() != typeof(GradientStop)) return false;
            return Equals((GradientStop)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Color.GetHashCode() * 397) ^ Offset.GetHashCode();
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
    }
}
