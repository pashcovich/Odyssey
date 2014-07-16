#region Using Directives

using Odyssey.Daedalus.Shaders.Structs;
using Odyssey.Engine;
using System.Collections.Generic;
using System.Text;

#endregion Using Directives

namespace Odyssey.Daedalus.Shaders.Nodes
{
    public class VSNormalTexturedTangentOutputNode : PhongVSOutputNode
    {
        [SupportedType(Type.Float4)]
        [IgnoreValidation(true)]
        public INode Tangent { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                foreach (var node in base.DescendantNodes)
                    yield return node;

                if (Tangent != null)
                {
                    yield return Tangent;
                    foreach (var node in Tangent.DescendantNodes)
                        yield return node;
                }
            }
        }

        protected override void PrintOutputStructure(StringBuilder sb, Struct vsOutput)
        {
            base.PrintOutputStructure(sb, vsOutput);
            if (Tangent != null)
                sb.AppendLine(string.Format("\t{0} = {1};", vsOutput[Param.SemanticVariables.Tangent].FullName,
                    Tangent.Reference));
        }
    }
}