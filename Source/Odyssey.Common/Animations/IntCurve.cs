using SharpDX;

namespace Odyssey.Animations
{
    public class IntCurve : AnimationCurve<IntKeyFrame>
    {
        public IntCurve()
        {
            Function = Linear;
        }

        public static object Discrete(IntKeyFrame start, IntKeyFrame end, float time)
        {
            return time < end.Time ? start.Value : end.Value;
        }

        public static object Linear(IntKeyFrame start, IntKeyFrame end, float time)
        {
            float newValue = Map(start.Time, end.Time, time);
            return (int)MathUtil.Lerp(start.Value, end.Value, newValue);
        }
    }
}
