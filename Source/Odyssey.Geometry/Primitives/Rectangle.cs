using System;
using SharpDX.Mathematics;
using Real = System.Single;
using Point = SharpDX.Mathematics.Vector2;

namespace Odyssey.Geometry.Primitives
{
    public struct Rectangle : IEquatable<Rectangle>, IFigure
    {
        public readonly Point TopLeft;
        public readonly Real Width;
        public readonly Real Height;

        public Rectangle(Real width, Real height): this(Point.Zero, width, height)
        { }

        public Rectangle(Point topLeft, Real width, Real height)
        {
            TopLeft = topLeft;
            Width = width;
            Height = height;
        }

        public static implicit operator RectangleF(Rectangle from)
        {
            return from == default(Rectangle)
                ? default(RectangleF)
                : new RectangleF(@from.TopLeft.X, @from.TopLeft.Y, @from.Width, @from.Height);
        }

        #region IEquatable<Rectangle>

        public bool Equals(Rectangle other)
        {
            return TopLeft == other.TopLeft && MathUtil.NearEqual(Width, other.Width) &&
                   MathUtil.NearEqual(Height, other.Height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Rectangle && Equals((Rectangle) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = TopLeft.GetHashCode();
                hashCode = (hashCode*397) ^ Width.GetHashCode();
                hashCode = (hashCode*397) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Rectangle left, Rectangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rectangle left, Rectangle right)
        {
            return !left.Equals(right);
        }

        #endregion


        #region IFigure

        Point IFigure.TopLeft
        {
            get { return TopLeft; }
        }

        Real IFigure.Width
        {
            get { return Width; }
        }

        Real IFigure.Height
        {
            get { return Height; }
        }

        FigureType IFigure.FigureType
        {
            get { return FigureType.Rectangle;}
        }
        #endregion

    
    }
}
