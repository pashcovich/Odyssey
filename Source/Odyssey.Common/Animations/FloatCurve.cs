using System;
using SharpDX;

namespace Odyssey.Animations
{
    public class FloatCurve : AnimationCurve<FloatKeyFrame>
    {
        public static float Linear(KeyFrame<float> start, KeyFrame<float> end, TimeSpan time)
        {
            float newValue = Map(start.Time, end.Time, time);

            return MathUtil.Lerp(start.Value, end.Value, newValue);
        }
    }
}
