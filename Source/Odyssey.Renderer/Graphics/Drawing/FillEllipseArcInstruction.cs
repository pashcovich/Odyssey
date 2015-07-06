using Odyssey.Geometry.Primitives;

namespace Odyssey.Graphics.Drawing
{
    public class FillEllipseArcInstruction : FillEllipseInstruction
    {
        private readonly float radFrom;
        private readonly float radTo;

        public FillEllipseArcInstruction(Ellipse ellipse, float radFrom, float radTo, int tessellation = Designer.EllipseTessellation) : base(ellipse, tessellation)
        {
            this.radFrom = radFrom;
            this.radTo = radTo;
        }

        public override void Execute(Designer designer)
        {
            designer.FillEllipseArc(Ellipse, radFrom, radTo, Tessellation);
        }
    }
}
