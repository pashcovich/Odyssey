using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Geometry
{
    public struct Size : IEquatable<Size>
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public Size(int width, int height)
            : this()
        {
            Width = width;
            Height = height;
        }

        public static Size Empty
        {
            get { return new Size(0, 0); }
        }

        public bool IsEmpty
        {
            get { return Width == 0 && Height == 0; }
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode() + Height.GetHashCode();
        }

        public bool Equals(Size other)
        {
            return this.Width == other.Width && this.Height == other.Height;
        }

        public static bool operator ==(Size sizeA, Size sizeB)
        {
            return sizeA.Equals(sizeB);
        }

        public static bool operator !=(Size sizeA, Size sizeB)
        {
            return !sizeA.Equals(sizeB);
        }

        public static implicit operator Size2 (Size size)
        {
            return new Size2(size.Width, size.Height);
        }
    }
}