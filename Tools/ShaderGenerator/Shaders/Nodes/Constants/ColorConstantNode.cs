using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Constants
{
    
    public class ColorConstantNode : NodeBase
    {
        [SupportedType(Type.Float4)]
        public IVariable Color { get; set; }

        [SupportedType(Type.Float4)]
        public override IVariable Output { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield break;
            }
        }

        public ColorConstantNode()
        {
            IsVerbose = false;
            Output = new Vector { Name = "color", Type = Shaders.Type.Float4 };
        }

        public override string Operation()
        {
            return string.Format("\t{0} {1} = {2};\n", Mapper.Map(Output.Type), Output.Name, Access());
        }

        public override string Access()
        {
            return Color.FullName;
        }
    }
}
