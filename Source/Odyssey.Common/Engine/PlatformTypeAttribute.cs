using System;

namespace Odyssey.Engine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PlatformTypeAttribute : Attribute
    {
        public PlatformTypeAttribute(Type platformType)
        {
            PlatformType = platformType;
        }

        public Type PlatformType { get; private set; }
    }
}