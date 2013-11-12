using Odyssey.Tools.ShaderGenerator.Shaders.Nodes;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [YamlTag("Matrix")]
    public class YamlMatrix : YamlVariable
    {
        public YamlMatrix() { }

        public YamlMatrix(Matrix matrix)
            : base(matrix)
        {
        }

        public override IVariable ToVariable()
        {
            Matrix m = ConvertYamlVariable<Matrix>(this);
            return m;
        }
    }
}
