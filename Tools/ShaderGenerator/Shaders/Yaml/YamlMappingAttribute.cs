using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Yaml
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple= false)]
    public class YamlMappingAttribute : Attribute
    {
        public System.Type MatchingType { get; private set; }

        public YamlMappingAttribute(System.Type matchingType)
        {
            MatchingType = matchingType;
        }
    }
}
