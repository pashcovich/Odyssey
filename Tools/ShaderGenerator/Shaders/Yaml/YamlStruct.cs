using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [YamlTag("Struct")]
    public class YamlStruct : YamlVariable
    {
        [YamlMember(4)]
        [DefaultValue(CustomType.None)]
        public CustomType customType { get; set; }

        [YamlMember(8)]
        [DefaultValue(null)]
        public YamlVariable[] fields { get; set; }

        public YamlStruct() { }

        public YamlStruct(Struct structVariable)
            : base(structVariable)
        {
            customType = structVariable.CustomType;
            if (structVariable.VariableCount == 0)
                return;
           
            fields = structVariable.Variables.Select(v => YamlShader.CurrentSerializer.RegisterVariable(v)).ToArray();
        }

        public override IVariable ToVariable()
        {
            return ConvertYamlStruct<Struct>(this);            
        }

        public static TStruct ConvertYamlStruct<TStruct>(YamlStruct yamlStruct)
            where TStruct : Struct, new()
        {
            var serializer = YamlShader.CurrentSerializer;
            TStruct s = ConvertYamlVariable<TStruct>(yamlStruct);
            s.CustomType = yamlStruct.customType;
            foreach (var v in yamlStruct.fields)
            {
                if (yamlStruct.type != Type.ConstantBuffer)
                    v.owner = yamlStruct;
                Variable variable = serializer.ContainsYamlVariable(v.fullName)
                    ? (Variable)serializer.GetVariable(v.fullName)
                    : (Variable)serializer.RegisterYamlVariable(v, s);
                variable.Owner = s;
                s.Add(variable);
            }
            return s;
        }

    }
}
