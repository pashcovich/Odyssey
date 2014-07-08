using Odyssey.Tools.ShaderGenerator.Shaders.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes.Functions
{
    public class TrinaryFunctionNode : BinaryFunctionNode
    {
        [DataMember]
        [SupportedType(Type.Float)]
        [SupportedType(Type.Float2)]
        [SupportedType(Type.Float3)]
        [SupportedType(Type.Float4)]
        [SupportedType(Type.Matrix)]
        public INode Input3 { get; set; }

        public override IEnumerable<INode> DescendantNodes
        {
            get
            {
                foreach (INode node in base.DescendantNodes)
                    yield return node;

                yield return Input3;
                foreach (INode node in Input3.DescendantNodes)
                    yield return node;
            }
        }

        public override string Access()
        {
            return Function.Call(Input1.Reference, Input2.Reference, Input3.Reference);
        }
    }
}