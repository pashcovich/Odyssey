using System;
using SharpDX;

namespace Odyssey.Animation
{
    public class FloatKeyFrame : KeyFrame<float>
    {
        public static float Linear(KeyFrame<float> start, KeyFrame<float> end, TimeSpan time)
        {
            float newValue = Map(start.Time, end.Time, time);

            return MathUtil.Lerp(start.Value, end.Value, newValue);
        }
    }
}
