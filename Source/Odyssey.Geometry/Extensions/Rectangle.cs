using System;
using SharpDX;
using Point = SharpDX.Vector2;
using Real = System.Single;

namespace Odyssey.Geometry.Extensions
{
    public static class Rectangle
    {
        public static RectangleF FromTwoPoints(Point p1, Point p2)
        {
            Real minX = Math.Min(p1.X, p2.X);
            Real minY = Math.Min(p1.Y, p2.Y);
            Real maxX = Math.Max(p1.X, p2.X);
            Real maxY = Math.Max(p1.Y, p2.Y);

            Real width = maxX - minX;
            Real height = maxY - minY;

            return new RectangleF(minX, minY, width, height);
        }
    }
}
