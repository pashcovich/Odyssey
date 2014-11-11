using System.Diagnostics;

namespace Odyssey.UserInterface.Style
{
    [DebuggerDisplay("{PrimitiveType} = ({arguments})")]
    public class VectorCommand
    {
        private readonly float[] arguments;
        private readonly PrimitiveType primitiveType;
        private readonly bool isRelative;

        public VectorCommand(PrimitiveType primitiveType, float[] arguments, bool isRelative = false)
        {
            this.primitiveType = primitiveType;
            this.arguments = arguments;
            this.isRelative = isRelative;
        }

        public float[] Arguments { get { return arguments; } }

        public PrimitiveType PrimitiveType { get { return primitiveType; } }

        public bool IsRelative
        {
            get { return isRelative; }
        }
    }
}