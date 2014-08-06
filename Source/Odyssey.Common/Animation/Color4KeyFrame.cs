using System;
using SharpDX;

namespace Odyssey.Animation
{
    public class Color4KeyFrame : KeyFrame<Color4>
    {
        public static Color4 Linear(Color4KeyFrame start, Color4KeyFrame end, TimeSpan time)
        {
            float newValue = Map(start.Time, end.Time, time);
            return Color4.Lerp(start.Value, end.Value, newValue);
        }

    }
}
