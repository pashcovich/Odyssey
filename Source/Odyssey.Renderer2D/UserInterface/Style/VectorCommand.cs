using System.Diagnostics;

namespace Odyssey.UserInterface.Style
{
    [DebuggerDisplay("{Type} = ({arguments})")]
    public class VectorCommand
    {
        private readonly float[] arguments;
        private readonly CommandType type;
        private readonly bool isRelative;

        public VectorCommand(CommandType type, float[] arguments, bool isRelative = false)
        {
            this.type = type;
            this.arguments = arguments;
            this.isRelative = isRelative;
        }

        public float[] Arguments { get { return arguments; } }

        public CommandType Type { get { return type; } }

        public bool IsRelative
        {
            get { return isRelative; }
        }
    }
}