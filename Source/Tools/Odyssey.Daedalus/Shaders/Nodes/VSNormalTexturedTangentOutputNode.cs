#region Using Directives

using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using System.Text;

#endregion Using Directives

namespace Odyssey.Daedalus.Shaders.Nodes
{
    public class VSNormalTexturedTangentOutputNode : PhongVSOutputNode
    {
        [SupportedType(Type.Float4)]
        [IgnoreValidation(true)]
        public INode Tangent { get; set; }

        protected override void PrintOutputStructure(StringBuilder sb, Struct vsOutput)
        {
            base.PrintOutputStructure(sb, vsOutput);
            if (Tangent != null)
                sb.AppendLine($"\t{vsOutput[Param.SemanticVariables.Tangent].FullName} = {Tangent.Reference};");
        }

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode(nameof(Tangent), Tangent);
        }
    }
}