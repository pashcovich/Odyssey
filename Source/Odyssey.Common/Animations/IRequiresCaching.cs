using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Animations
{
    public interface IRequiresCaching
    {
        IAnimationCurve CacheAnimation(string property, IAnimationCurve animationCurve);
    }
}
