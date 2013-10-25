using SharpDX;

namespace Odyssey.UserInterface
{
    public static class Extensions
    {
        public static Vector2 ToVector2(this Windows.Foundation.Point point)
        {
            return new Vector2((float)point.X, (float)point.Y);
        }

    }
}
