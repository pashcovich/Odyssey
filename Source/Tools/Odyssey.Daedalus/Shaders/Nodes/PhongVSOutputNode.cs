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
            Struct vsOutput = (Struct)Output;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t{0} {1};", vsOutput.CustomType, vsOutput.Name));

            PrintOutputStructure(sb, vsOutput);
            PrintReturnOutput(sb, vsOutput);

            return sb.ToString();
        }

        protected virtual void PrintOutputStructure(StringBuilder sb, Struct vsOutput)
        {
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Position].FullName, Position.Reference));
            if (Normal != null)
                sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Normal].FullName, Normal.Reference));
            if (WorldPosition != null)
                sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.WorldPosition].FullName, WorldPosition.Reference));
            if (Texture != null)
                sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Texture].FullName, Texture.Reference));
        }

        protected virtual void PrintReturnOutput(StringBuilder sb, Struct vsOutput)
        {
            sb.AppendLine(string.Format("\treturn {0};", vsOutput.Name));
        }

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode("WorldPosition", WorldPosition);
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
            Struct vsOutput = (Struct)Output;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t{0} {1};", vsOutput.CustomType, vsOutput.Name));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Position].FullName, Position.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Normal].FullName, Normal.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.WorldPosition].FullName, WorldPosition.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.ShadowProjection].FullName, ShadowProjection.Reference));
            sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Texture].FullName, Texture.Reference));
            sb.AppendLine(string.Format("\treturn {0};", vsOutput.Name));

            return sb.ToString();
        }

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode("ShadowProjection", ShadowProjection);
        }
    }
}