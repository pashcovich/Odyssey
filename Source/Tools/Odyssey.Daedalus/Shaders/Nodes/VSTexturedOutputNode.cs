using System.Collections.Generic;
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
            var vsOutput = (Struct)Output;
            var sb = new StringBuilder();
            sb.AppendLine($"\t{vsOutput.CustomType} {vsOutput.Name};");
            sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.Position].FullName} = {Position.Reference};");
            sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.Texture].FullName} = {Texture.Reference};");
            sb.AppendLine($"\treturn {vsOutput.Name};");

            return sb.ToString();
        }

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode(nameof(Texture), Texture);
        }
    }
}