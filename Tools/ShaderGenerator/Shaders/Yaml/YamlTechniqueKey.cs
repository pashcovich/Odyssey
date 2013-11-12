using Odyssey.Graphics.Materials;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [YamlTag("TechniqueKey")]
    public class YamlTechniqueKey
    {
        [YamlMember(0)]
        public ShaderModel shaderModel { get; set; }

        [DefaultValue(VertexShaderFlags.None)]
        [YamlMember(1)]
        public VertexShaderFlags vertexShader { get; set; }

        [DefaultValue(PixelShaderFlags.None)]
        [YamlMember(2)]
        public PixelShaderFlags pixelShader { get; set; }

        public YamlTechniqueKey()
        {}

        public YamlTechniqueKey(TechniqueKey key)
        {
            shaderModel = key.ShaderModel;
            vertexShader = key.VertexShader;
            pixelShader = key.PixelShader;
        }

        public TechniqueKey ToTechniqueKey()
        {
            return new TechniqueKey(vertexShader, pixelShader, shaderModel);
        }
    }
}
