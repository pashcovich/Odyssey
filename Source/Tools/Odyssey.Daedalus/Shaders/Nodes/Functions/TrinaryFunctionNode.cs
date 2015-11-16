using System.Runtime.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Functions
{
    public class TrinaryFunctionNode : BinaryFunctionNode
    {
        [DataMember]
        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        public INode Input3 { get; set; }

        public override string Access()
        {
            return Function.Call(Input1.Reference, Input2.Reference, Input3.Reference);
        }

        protected override void RegisterNodes()
        {
            base.RegisterNodes();
            AddNode("Input3", Input3);
        }
    }
}