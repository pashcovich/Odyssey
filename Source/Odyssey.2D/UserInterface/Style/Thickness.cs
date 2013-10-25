using System;

namespace Odyssey.UserInterface.Style
{
    public struct Thickness : IEquatable<Thickness>
    {
        private readonly bool symmetricPadding;

        public Thickness(int padding)
            : this()
        {
            symmetricPadding = true;
            Left = Top = Right = Bottom = padding;
        }

        public Thickness(int left, int top, int right, int bottom)
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
        public int All
        {
            get
            {
                if (symmetricPadding)
                    return Top;
                else
                    return -1;
            }
        }

        public int Bottom { get; set; }
        public int Horizontal
        {
            get { return Left + Right; }
        }
        public bool IsEmpty
        {
            get
            {
                return All == 0 || (Top == 0 && Right == 0 && Bottom == 0 && Left == 0);
            }
        }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Vertical
        {
            get { return Top + Bottom; }
        }

        public static bool operator !=(Thickness padding1, Thickness padding2)
        {
            return !padding1.Equals(padding2);
        }

        public static bool operator ==(Thickness padding1, Thickness padding2)
        {
            return padding1.Equals(padding2);
        }

        public bool Equals(Thickness other)
        {
            return other.symmetricPadding.Equals(symmetricPadding) && other.Left == Left && other.Top == Top && other.Right == Right && other.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Thickness)) return false;
            return Equals((Thickness)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = symmetricPadding.GetHashCode();
                result = (result * 397) ^ Left;
                result = (result * 397) ^ Top;
                result = (result * 397) ^ Right;
                result = (result * 397) ^ Bottom;
                return result;
            }
        }
    }
}