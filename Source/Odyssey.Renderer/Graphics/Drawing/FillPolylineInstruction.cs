using System.Collections.Generic;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public class FillPolylineInstruction : IDesignerInstruction
    {
        private readonly IEnumerable<Vector2> points;
        private readonly float lineWidth;
        private readonly PolygonDirection direction;

        public FillPolylineInstruction(IEnumerable<Vector2> points, float lineWidth, PolygonDirection direction=PolygonDirection.PositiveY)
        {
            this.points = points;
            this.lineWidth = lineWidth;
            this.direction = direction;
        }

        public void Execute(Designer designer)
        {
            designer.FillPolyline(points, lineWidth, direction);
        }
    }
}
