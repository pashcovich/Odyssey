using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PlatformTypeAttribute : Attribute
    {
        public Type PlatformType { get; private set; }

        public PlatformTypeAttribute(Type platformType)
        {
            PlatformType = platformType;
        }
    }
}
