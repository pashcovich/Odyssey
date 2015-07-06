using System.Collections.Generic;

namespace Odyssey.Graphics.Drawing
{
    public class InstructionCollection : List<IDesignerInstruction>
    {
        public InstructionCollection()
        {
        }

        public InstructionCollection(int capacity)
            : base(capacity)
        {
        }

        public InstructionCollection(IEnumerable<IDesignerInstruction> collection)
            : base(collection)
        {
        }
    }
}
