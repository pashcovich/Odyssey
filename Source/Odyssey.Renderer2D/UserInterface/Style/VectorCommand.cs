using System.Diagnostics;

namespace Odyssey.UserInterface.Style
{
    [DebuggerDisplay("{command} = ({arguments})")]
    public class VectorCommand
    {
        private readonly float[] arguments;
        private readonly char command;

        public VectorCommand(char command, float[] arguments)
        {
            this.command = command;
            this.arguments = arguments;
        }

        public float[] Arguments { get { return arguments; } }

        public char Command { get { return command; } }
    }
}