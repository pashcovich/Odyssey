using Odyssey.Graphics.Models;

namespace Odyssey.Graphics.Drawing
{
    public class SetModelOperationsInstruction : IDesignerInstruction
    {
        private readonly ModelOperation modelOperations;
        public ModelOperation ModelOperations { get { return modelOperations; } }

        public SetModelOperationsInstruction(ModelOperation modelOperations)
        {
            this.modelOperations = modelOperations;
        }

        public void Execute(Designer designer)
        {
            designer.ModelOperations = modelOperations;
        }
    }
}
