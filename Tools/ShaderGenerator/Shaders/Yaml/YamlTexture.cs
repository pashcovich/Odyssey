using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [YamlTag("Texture")]
    public class YamlTexture : YamlVariable
    {
        public YamlTexture() { }

        public YamlTexture(Texture texture)
            : base(texture)
        {}

        public override IVariable ToVariable()
        {
            Texture t = ConvertYamlVariable<Texture>(this);
            return t;
        }
    }
}
