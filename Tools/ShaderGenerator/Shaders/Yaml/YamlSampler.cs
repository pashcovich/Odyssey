using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [YamlTag("Sampler")]
    public class YamlSampler : YamlVariable
    {
        public YamlSampler()
        {}

        public YamlSampler(Sampler sampler)
            : base(sampler)
        {}

        public override IVariable ToVariable()
        {
            Sampler s = ConvertYamlVariable<Sampler>(this);
            return s;
        }
    }
}
