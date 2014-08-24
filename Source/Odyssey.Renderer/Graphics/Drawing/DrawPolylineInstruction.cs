using System.Collections.Generic;
using SharpDX;

namespace Odyssey.Graphics.Drawing
{
    public class DrawPolylineInstruction : DrawInstruction
    {
        private readonly IEnumerable<Vector2> points;

        public DrawPolylineInstruction(IEnumerable<Vector2> points, float strokeThickness) : base(strokeThickness)
        {
            this.points = points;
        }

        public override void Execute(Designer designer)
        {
            designer.DrawPolyLine(points, StrokeThickness);
        }
    }
}
