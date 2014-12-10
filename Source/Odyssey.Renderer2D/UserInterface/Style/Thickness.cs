#region Using Directives

using System;
using System.Diagnostics.Contracts;

#endregion

namespace Odyssey.UserInterface.Style
{
    public struct Thickness : IEquatable<Thickness>
    {
        private readonly bool symmetricPadding;

        public Thickness(float padding)
            : this()
        {
            symmetricPadding = true;
            Left = Top = Right = Bottom = padding;
        }

        public Thickness(float left, float top, float right, float bottom)
            : this()
        {
            symmetricPadding = false;
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public static Thickness Default
        {
            get { return new Thickness(5); }
        }

        public static Thickness Empty
        {
            get { return new Thickness(0); }
        }

        public float All
        {
            get
            {
                if (symmetricPadding)
                    return Top;
                return -1;
            }
        }

        public float Bottom { get; set; }

        public float Horizontal
        {
            get { return Left + Right; }
        }

        public bool IsEmpty
        {
            get { return All == 0 || (Top == 0 && Right == 0 && Bottom == 0 && Left == 0); }
        }

        public float Left { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }

        public float Vertical
        {
            get { return Top + Bottom; }
        }

        [Pure]
        public bool Equals(Thickness other)
        {
            return other.symmetricPadding.Equals(symmetricPadding) && other.Left == Left && other.Top == Top && other.Right == Right && other.Bottom == Bottom;
        }

        public static bool operator !=(Thickness padding1, Thickness padding2)
        {
            return !padding1.Equals(padding2);
        }

        public static bool operator ==(Thickness padding1, Thickness padding2)
        {
            return padding1.Equals(padding2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Thickness)) return false;
            return Equals((Thickness) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = symmetricPadding.GetHashCode();
                result = (result*397) ^ (int) Left;
                result = (result*397) ^ (int) Top;
                result = (result*397) ^ (int) Right;
                result = (result*397) ^ (int) Bottom;
                return result;
            }
        }
    }
}