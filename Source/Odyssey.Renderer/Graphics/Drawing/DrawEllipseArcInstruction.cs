using Odyssey.Geometry.Primitives;

namespace Odyssey.Graphics.Drawing
{
    public class DrawEllipseArcInstruction : DrawEllipseInstruction
    {
        private readonly float radFrom;
        private readonly float radTo;
        private readonly float lineWidth;

        private readonly int tessellation;

        public int Tessellation
        {
            get { return tessellation; }
        }

        public DrawEllipseArcInstruction(Ellipse ellipse, float radFrom, float radTo, int tessellation, float strokeWidth)
            : base(ellipse, strokeWidth)
        {
            this.radFrom = radFrom;
            this.radTo = radTo;
            this.tessellation = tessellation;
        }

        public override void Execute(Designer designer)
        {
            designer.DrawEllipseArc(Ellipse, radFrom, radTo, tessellation, lineWidth, StrokeThickness);
        }
    }
}
