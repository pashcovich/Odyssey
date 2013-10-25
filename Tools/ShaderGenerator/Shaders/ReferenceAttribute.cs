using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ReferenceAttribute : Attribute
    {
        ReferenceType type;
        object referenceValue;

        public ReferenceType Type { get { return type; } set { type = value; } }
        public object Value { get { return referenceValue; } set { referenceValue = value; } }

        ReferenceAttribute(ReferenceType type, object value)
        {
            Type = type;
            Value = value;
        }

        public ReferenceAttribute(EngineReference reference)
            : this(ReferenceType.Engine, reference)
        {}

        public ReferenceAttribute(TextureReference reference)
            : this(ReferenceType.Texture, reference)
        { }
    }
}
