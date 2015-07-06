using Odyssey.Geometry.Primitives;

namespace Odyssey.Graphics.Drawing
{
    public class FillTriangleInstruction : IDesignerInstruction
    {
        private readonly Triangle triangle;

        public FillTriangleInstruction(Triangle triangle)
        {
            this.triangle = triangle;
        }

        public void Execute(Designer designer)
        {
            designer.FillTriangle(triangle);
        }
    }
}
