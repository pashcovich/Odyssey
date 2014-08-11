using System;

namespace Odyssey.Animations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CacheAnimationAttribute : Attribute
    {
        public Type Type { get; private set; }
        public string PropertyName { get; private set; }

        public CacheAnimationAttribute(Type type, string propertyName)
        {
            Type = type;
            PropertyName = propertyName;
        }
    }
}
