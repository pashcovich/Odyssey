using System.Collections.Generic;
using System.Linq;
using SharpDX.Serialization;

namespace Odyssey.Daedalus.Shaders.Methods
{
    public struct MethodReference : IDataSerializable
    {
        private string[] arguments;
        public IMethod Method { get; private set; }

        public IEnumerable<string> Arguments
        {
            get { return arguments; }
        }

        public MethodReference(IMethod method, IEnumerable<string> arguments) : this()
        {
            Method = method;
            this.arguments = arguments.ToArray();
        }

        public string Call()
        {
            return Method.Call(Arguments.ToArray());
        }

        public void Serialize(BinarySerializer serializer)
        {
            if (serializer.Mode == SerializerMode.Write)
                MethodBase.WriteMethod(serializer, Method);
            else Method = MethodBase.ReadMethod(serializer);

            serializer.Serialize(ref arguments, serializer.Serialize);
        }
    }
}
