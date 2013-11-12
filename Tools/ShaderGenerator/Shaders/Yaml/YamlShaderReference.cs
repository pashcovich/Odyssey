using Odyssey.Graphics.Materials;
using ShaderGenerator.Data;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [YamlTag("ShaderReference")]
    public class YamlShaderReference
    {
        [DefaultValue(0)]
        public int index { get; set; }
        [YamlIgnore]
        public ReferenceType type { get;  set; }
        public object value { get; set; }

        public YamlShaderReference()
        {}

        public YamlShaderReference(ShaderReference reference)
        {
            index = reference.Index;
            type = reference.Type;
            value = reference.Value;
        }

        public ShaderReference ToShaderReference()
        {
            switch (type)
            {
                case ReferenceType.Engine:
                    if (value.GetType() == typeof(EngineReference))
                        value = (EngineReference)value;
                    else
                        value = (EngineReference)Enum.Parse(typeof(EngineReference), (string)value);
                    break;
                case ReferenceType.Texture:
                    value = (TextureReference)value;
                    //value = (TextureReference)Enum.Parse(typeof(TextureReference), (string)value);
                    break;
            }
            return new ShaderReference(type, value, index);
        }


    
    }
}
