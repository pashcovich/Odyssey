using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Animations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CacheAnimationAttribute : Attribute
    {
        public CacheAnimationAttribute(bool value = true)
        { }
    }
}
