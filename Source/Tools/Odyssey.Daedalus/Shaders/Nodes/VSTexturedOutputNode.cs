using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Daedalus.Shaders.Nodes
{
    [DataContract]
    public class VSTexturedOutputNode : VSOutputNodeBase
    {
        [DataMember]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [IgnoreValidation(true)]
        public INode Texture { get; set; }

        public override string Operation(ref int indentation)
        {
            Struct vsOutput = (Struct)Output;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t{0} {1};", vsOutput.CustomType, vsOutput.Name));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Position].FullName, Position.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Texture].FullName, Texture.Reference));
            sb.AppendLine(string.Format("\treturn {0};", vsOutput.Name));

            return sb.ToString();
        }

        protected override void RegisterNodes()
        {
            AddNode("Texture", Texture);
        }
    }
}