using Odyssey.Serialization;

namespace Odyssey.Daedalus.Shaders.Nodes.Operators
{
    public class ArrayNode : NodeBase
    {
        private IVariable output;
        private string index;

        public string Index
        {
            get { return index; }
            set { index = value; }
        }

        [SupportedType(Type.Float)]
        public override IVariable Output
        {
            get
            {
                if (output != null) return output;

                const Type type = Type.Float;
                string name = string.Format("{0}{1}", Input.Name, Index);
                Output = Variable.InitVariable(name, type);
                return output;

            }
            set { output = value; }
        }

        [SupportedType(Type.Vector)]
        [SupportedType(Type.Matrix)]
        public IVariable Input { get; set; }

        public override string Access()
        {
            return string.Format("{0}[{1}]", Input.FullName, Index);
        }

        protected override void SerializeProperties(BinarySerializer serializer)
        {
            base.SerializeProperties(serializer);
            serializer.Serialize(ref index);
        }

        protected override void SerializeVariables(BinarySerializer serializer)
        {
            base.SerializeVariables(serializer);
            if (serializer.Mode == SerializerMode.Write)
                Variable.WriteVariable(serializer, Input);
            else
                Input = Variable.ReadVariable(serializer);
        }

        protected override void RegisterNodes()
        {
        }
    }
}
