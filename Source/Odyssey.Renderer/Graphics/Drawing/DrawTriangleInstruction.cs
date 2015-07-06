using Odyssey.Geometry.Primitives;

namespace Odyssey.Graphics.Drawing
{
    public class DrawTriangleInstruction : DrawInstruction
    {
        private readonly Triangle triangle;

        public DrawTriangleInstruction(Triangle triangle, float strokeThickness) : base(strokeThickness)
        {
            this.triangle = triangle;
        }

        public override void Execute(Designer designer)
        {
            designer.DrawTriangle(triangle, StrokeThickness);
        }
    }
}
