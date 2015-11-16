using Odyssey.Serialization;

namespace Odyssey.Daedalus.Shaders.Methods
{
    public class IntrinsicFunction : MethodBase
    {
        private int arguments;
        public int Arguments => arguments;

        public IntrinsicFunction() : base()
        {
            arguments = 0;
            Name = "Undefined";
        }

        public IntrinsicFunction(string methodName, int arguments) : base(true)
        {
            Name = methodName;
            this.arguments = arguments;
        }
        
        public override string Body => "Intrinsic Functions do not have a body";

        public override void Serialize(BinarySerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Serialize(ref arguments);
        }
    }
}
