using System.Collections.Generic;
using System.Linq;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public class DrawPolylineInstruction : DrawInstruction
    {
        private readonly IEnumerable<Vector2> points;
        private readonly float lineWidth;
        private readonly PolygonDirection direction;

        public DrawPolylineInstruction(IEnumerable<Vector2> points, float lineWidth, float strokeThickness,
            PolygonDirection direction = PolygonDirection.PositiveY) : base(strokeThickness)
        {
            this.points = points;
            this.lineWidth = lineWidth;
            this.direction = direction;
        }

        public Vector2[] Points { get { return points.ToArray(); } }

        public override void Execute(Designer designer)
        {
            designer.DrawPolyline(points, lineWidth, StrokeThickness, direction);
        }
    }
}
