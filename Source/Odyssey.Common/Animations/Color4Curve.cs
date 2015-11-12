#region Using Directives

using SharpDX;

#endregion

namespace Odyssey.Animations
{
    public class Color4Curve : AnimationCurve<Color4KeyFrame>
    {
        public Color4Curve()
        {
            Function = Discrete;
        }

        public static object Linear(Color4KeyFrame start, Color4KeyFrame end, float time, object options = null)
        {
            float newValue = Map(start.Time, end.Time, time);
            return Color4.Lerp(start.Value, end.Value, newValue);
        }

        public new static object Discrete(Color4KeyFrame start, Color4KeyFrame end, float time,  object options = null)
        {
            return end.Value;
        }
    }
}