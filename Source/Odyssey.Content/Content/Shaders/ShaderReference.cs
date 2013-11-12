using Odyssey.Content.Shaders;
using Odyssey.Graphics.Materials;
using System;
using System.Runtime.Serialization;

namespace ShaderGenerator.Data
{
    [DataContract]
    public class ShaderReference 
    {
        [DataMember]
        public int Index { get; set; }
        [DataMember]
        public ReferenceType Type { get; private set; }
        [DataMember]
        public object Value { get; private set; }

        protected internal ShaderReference(ReferenceType type, object value, int index = 0)
        {
            Type = type;
            Value = value;
        }

        public ShaderReference()
        {}

        public ShaderReference(EngineReference reference)
            : this(ReferenceType.Engine, reference)
        {}

        public ShaderReference(TextureReference reference)
            : this(ReferenceType.Texture, reference)
        { }
    }
}
