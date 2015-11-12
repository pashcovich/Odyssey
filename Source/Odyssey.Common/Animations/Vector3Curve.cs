using SharpDX;

namespace Odyssey.Animations
{
    public class Vector3Curve : AnimationCurve<Vector3KeyFrame>
    {
        public static object Linear(Vector3KeyFrame start, Vector3KeyFrame end, float time, object options = null)
        {
            float newValue = Map(start.Time, end.Time, time);
            return Vector3.Lerp(start.Value, end.Value, newValue);
        }
    }
}
