using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
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
            var psOutput = (Struct)Output;
            var sb = new StringBuilder();
            sb.AppendLine($"\t{psOutput.CustomType} {psOutput.Name};");
            sb.AppendLine($"\t{psOutput[Param.SemanticVariables.Color].FullName} = {FinalColor.Reference};");
            sb.AppendLine($"\treturn {psOutput.Name};");

            return sb.ToString();
        }

        public override string Access()
        {
            return string.Empty;
        }

        protected override void RegisterNodes()
        {
            AddNode(nameof(FinalColor), FinalColor);
        }
    }
}