using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public class SetTransformInstruction : IDesignerInstruction
    {
        private readonly Matrix transform;
        public Matrix Transform
        {
            get { return transform; }
        }

        public SetTransformInstruction(Matrix transform)
        {
            this.transform = transform == default(Matrix) ? Matrix.Identity : transform;
        }

        public void Execute(Designer designer)
        {
            designer.Transform = Transform;
        }
    }
}
