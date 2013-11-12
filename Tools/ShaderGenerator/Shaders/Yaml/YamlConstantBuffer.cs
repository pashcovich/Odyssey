using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [YamlTag("ConstantBuffer")]
    public class YamlConstantBuffer : YamlStruct
    {
        [YamlMember(4)]
        public UpdateFrequency updateFrequency { get; set; }

        public YamlConstantBuffer()
        {}

        public YamlConstantBuffer(ConstantBuffer constantBuffer)
            : base(constantBuffer)
        {
            updateFrequency = constantBuffer.UpdateFrequency;
        }

        public override IVariable ToVariable()
        {
            ConstantBuffer cb = ConvertYamlStruct<ConstantBuffer>(this);
            cb.UpdateFrequency = this.updateFrequency;
            return cb;
        }
    }
}
