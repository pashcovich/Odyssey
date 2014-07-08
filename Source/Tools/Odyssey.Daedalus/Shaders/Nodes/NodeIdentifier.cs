using SharpDX.Serialization;

namespace Odyssey.Tools.ShaderGenerator.Shaders.Nodes
{
    internal struct NodeIdentifier : IDataSerializable
    {
        public IVariable Variable;
        public Type SupportedType;
        public INode Node;

        public void Serialize(BinarySerializer serializer)
        {
            if (serializer.Mode == SerializerMode.Write)
                Shaders.Variable.WriteVariable(serializer, Variable);
            else
                Variable = Shaders.Variable.ReadVariable(serializer);

            serializer.SerializeEnum(ref SupportedType);

            if (serializer.Mode == SerializerMode.Write)
                NodeBase.WriteNode(serializer, Node);
            else
                Node = NodeBase.ReadNode(serializer);
        }
    }
}