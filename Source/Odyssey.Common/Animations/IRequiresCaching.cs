using System;

namespace Odyssey.Animations
{
    public interface IRequiresCaching
    {
        IAnimationCurve CacheAnimation(Type type, string property, IAnimationCurve animationCurve);
    }
}
