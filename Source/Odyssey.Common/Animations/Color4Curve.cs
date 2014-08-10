using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace Odyssey.Animations
{
    public class Color4Curve : AnimationCurve<Color4KeyFrame>
    {
        public Color4Curve()
        {
            Function = Discrete;
        }

        public static object Linear(Color4KeyFrame start, Color4KeyFrame end, TimeSpan time)
        {
            float newValue = Map(start.Time, end.Time, time);
            return Color4.Lerp(start.Value, end.Value, newValue);
        }

        public static object Discrete(Color4KeyFrame start, Color4KeyFrame end, TimeSpan time)
        {
            return end.Value;
        }
    }
}
