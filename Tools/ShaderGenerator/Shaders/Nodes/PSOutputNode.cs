using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    [DataContract]
    public class PSOutputNode : NodeBase
    {
        [DataMember]
        [SupportedType(Type.Float4)]
        public INode FinalColor { get; set; }
        
        [DataMember]
        [SupportedType(Type.Struct)]
        public override IVariable Output { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                yield return FinalColor;
                foreach (var node in FinalColor.DescendantNodes)
                    yield return node;
            }
        }

        public PSOutputNode()
        {
            IsVerbose = true;
        }

        public override string Operation()
        {
            Struct psOutput = (Struct)Output;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine( string.Format("\t{0} {1};", psOutput.CustomType, psOutput.Name));
            sb.AppendLine(string.Format("\t{0} = {1};", psOutput[Param.SemanticVariables.Color].FullName, 
                FinalColor.Reference));
            sb.AppendLine(string.Format("\treturn {0};", psOutput.Name));

            return sb.ToString();
        }

        public override string Access()
        {
            return string.Empty;
        }
    }
}
