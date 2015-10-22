using System.Collections.Generic;
using System.Linq;
using SharpDX.Mathematics;

namespace Odyssey.Graphics.Drawing
{
    public class DrawPolylineInstruction : DrawInstruction
    {
        private readonly IEnumerable<Vector2> points;
        private readonly FaceDirection direction;
        private readonly bool closed;

        public DrawPolylineInstruction(IEnumerable<Vector2> points, float strokeThickness, FaceDirection direction = FaceDirection.PositiveY, bool closed = false) : base(strokeThickness)
        {
            this.points = points;
            this.direction = direction;
            this.closed = closed;
        }

        public Vector2[] Points { get { return points.ToArray(); } }

        public override void Execute(Designer designer)
        {
            if (closed)
                designer.DrawClosedPolyline(points, StrokeThickness, direction);
            else
                designer.DrawPolyline(points, StrokeThickness, direction);
            
        }
    }
}
