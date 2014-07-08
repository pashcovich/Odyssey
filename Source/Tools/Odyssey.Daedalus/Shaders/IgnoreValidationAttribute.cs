using System;

namespace Odyssey.Tools.ShaderGenerator.Shaders
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
