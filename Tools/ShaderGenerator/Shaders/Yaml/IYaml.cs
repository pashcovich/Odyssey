using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    public interface IYamlVariable
    {
        YamlVariable ToYaml();
    }

    public interface IYamlNode
    {
        YamlNode ToYaml();
    }
}
