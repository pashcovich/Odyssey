using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
