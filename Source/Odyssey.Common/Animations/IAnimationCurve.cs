using System;

namespace Odyssey.Animations
{
    public interface IAnimationCurve {
        string TargetProperty { get; }
        string TargetName { get; }

        float Duration { get; }
        object Evaluate(TimeSpan elapsedTime);
    }
}