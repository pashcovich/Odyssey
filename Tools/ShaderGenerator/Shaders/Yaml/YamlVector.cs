using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [YamlTag("Vector")]
    public class YamlVector : YamlVariable
    {
        [DefaultValue(null)]
        public float[] value { get; set; }
        [DefaultValue(null)]
        public Swizzle[] swizzle { get; set; }

        public YamlVector() { }

        public YamlVector(Vector vector)
            : base(vector)
        {
            value = vector.Value;
            swizzle = vector.Swizzle;
        }

        public override IVariable ToVariable()
        {
            Vector v = ConvertYamlVariable<Vector>(this);
            if (swizzle != null)
                v.Swizzle = swizzle;
            v.Value = value;
            return v;
        }

    }
}
