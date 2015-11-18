using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using System.Runtime.Serialization;
using System.Text;

namespace Odyssey.Daedalus.Shaders.Nodes
{
    public class PhongVSOutputNode : VSNormalTexturedOutputNode
    {
        [SupportedType(Type.Float4)]
        [IgnoreValidation(true)]
        public INode WorldPosition { get; set; }

        public override string Operation(ref int indentation)
        {
            var vsOutput = (Struct)Output;
            var sb = new StringBuilder();
            sb.AppendLine($"\t{vsOutput.CustomType} {vsOutput.Name};");

            PrintOutputStructure(sb, vsOutput);
            PrintReturnOutput(sb, vsOutput);

            return sb.ToString();
        }

        protected virtual void PrintOutputStructure(StringBuilder sb, Struct vsOutput)
        {
            sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.Position].FullName} = {Position.Reference};");
            if (Normal != null)
                sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.Normal].FullName} = {Normal.Reference};");
            if (WorldPosition != null)
                sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.WorldPosition].FullName} = {WorldPosition.Reference};");
            if (Texture != null)
                sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.Texture].FullName} = {Texture.Reference};");
        }

        protected virtual void PrintReturnOutput(StringBuilder sb, Struct vsOutput)
        {
            sb.AppendLine($"\treturn {vsOutput.Name};");
        }

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode(nameof(WorldPosition), WorldPosition);
        }
    }

    [DataContract]
    public class PhongShadowsVSOutputNode : PhongVSOutputNode
    {
        [DataMember]
        [SupportedType(Type.Float4)]
        public INode ShadowProjection { get; set; }

        public override string Operation(ref int indentation)
        {
            var vsOutput = (Struct)Output;
            var sb = new StringBuilder();
            sb.AppendLine($"\t{vsOutput.CustomType} {vsOutput.Name};");
            sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.Position].FullName} = {Position.Reference};");
            sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.Normal].FullName} = {Normal.Reference};");
            sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.WorldPosition].FullName} = {WorldPosition.Reference};");
            sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.ShadowProjection].FullName} = {ShadowProjection.Reference};");
            sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.Texture].FullName} = {Texture.Reference};");
            sb.AppendLine($"\treturn {vsOutput.Name};");

            return sb.ToString();
        }

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode(nameof(ShadowProjection), ShadowProjection);
        }
    }
}