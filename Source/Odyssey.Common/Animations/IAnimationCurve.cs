using System;

namespace Odyssey.Animations
{
    public interface IAnimationCurve {
        string TargetProperty { get; }
        string TargetName { get; }
        string Name { get; set; }

        float Duration { get; }
        object Evaluate(TimeSpan elapsedTime, bool forward = true);
    }
}