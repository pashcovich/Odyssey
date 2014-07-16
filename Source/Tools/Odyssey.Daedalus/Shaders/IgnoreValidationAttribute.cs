using System;

namespace Odyssey.Daedalus.Shaders
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreValidationAttribute : Attribute
    {
        public bool Value { get; set;}

        public IgnoreValidationAttribute(bool value)
        {
            Value = value;
        }
    }
}
