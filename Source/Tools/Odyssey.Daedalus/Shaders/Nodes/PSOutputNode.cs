using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using SharpDX.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Daedalus.Shaders.Nodes
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

        public PSOutputNode()
        {
            IsVerbose = true;
        }

        public override string Operation(ref int indentation)
        {
            Struct psOutput = (Struct)Output;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t{0} {1};", psOutput.CustomType, psOutput.Name));
            sb.AppendLine(string.Format("\t{0} = {1};", psOutput[Param.SemanticVariables.Color].FullName,
                //"cNormalMap"));
                FinalColor.Reference));
            sb.AppendLine(string.Format("\treturn {0};", psOutput.Name));

            return sb.ToString();
        }

        public override string Access()
        {
            return string.Empty;
        }

        protected override void RegisterNodes()
        {
            AddNode("FinalColor", FinalColor);
        }
    }
}