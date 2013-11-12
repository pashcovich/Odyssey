using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;

using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlNode = Odyssey.Tools.ShaderGenerator.Shaders.Yaml.YamlNode;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [YamlTag("Shader")]
    internal class YamlShader
    {
        static Type[] requiredVariables = new[] { Type.ConstantBuffer, Type.Sampler, Type.SamplerComparisonState, 
            Type.Texture2D, Type.Texture3D,Type.TextureCube
            };

        internal static YamlSerializer CurrentSerializer { get; set; }

        [YamlMember(0)]
        public string name { get; set; }
        [YamlMember(1)]
        public ShaderType type { get; set; }
        [YamlMember(2)]
        public YamlTechniqueKey key { get; set; }
        [DefaultValue(false)]
        [YamlMember(3)]
        public bool enableSeparators { get; set; }
        [YamlMember(4)]
        public YamlStruct inputStruct { get; set; }
        [YamlMember(5)]
        public YamlStruct outputStruct { get; set; }
        [DefaultValue(null)]
        [YamlMember(6)]
        public Dictionary<string, YamlVariable> variables { get; set; }
        [YamlMember(7)]
        public YamlNode result { get; set; }

        [YamlIgnore]
        public FeatureLevel featureLevel { get; set; }

        public YamlShader()
        { }

        internal YamlShader(Shader shader)
        {
            YamlSerializer.Clear();
            name = shader.Name;
            type = shader.Type;
            key = new YamlTechniqueKey(shader.KeyPart);
            featureLevel = Shader.FromShaderModel(key.shaderModel, type);
            enableSeparators = shader.EnableSeparators;
            inputStruct = (YamlStruct)CurrentSerializer.RegisterVariable(shader.InputStruct);
            outputStruct = (YamlStruct)CurrentSerializer.RegisterVariable(shader.OutputStruct);
            variables = (from v in shader.Variables
                         where requiredVariables.Contains(v.Type)
                         select v).ToDictionary(v => v.Name, v => ((IYamlVariable)v).ToYaml());

            result = ((IYamlNode)shader.Result).ToYaml();
            
        }

        public Shader ToShader()
        {
            YamlSerializer.Clear();
            Shader shader = new Shader
            {
                Name = this.name,
                Type = this.type,
                KeyPart = this.key.ToTechniqueKey(),
                FeatureLevel = Shader.FromShaderModel(this.key.shaderModel, this.type),
                EnableSeparators = this.enableSeparators,
                InputStruct = (IStruct)this.inputStruct.ToVariable(),
                OutputStruct = (IStruct)this.outputStruct.ToVariable(),
            };
            shader.Add(from v in this.variables.Values select v.ToVariable());
            shader.Result = this.result.ToNode();

            return shader;

        }

        

    }
}
