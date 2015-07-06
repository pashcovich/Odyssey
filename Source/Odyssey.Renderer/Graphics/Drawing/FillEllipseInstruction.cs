using Odyssey.Geometry.Primitives;

namespace Odyssey.Graphics.Drawing
{
    public class FillEllipseInstruction : IDesignerInstruction
    {
        private readonly Ellipse ellipse;
        private readonly int tessellation;

        public int Tessellation
        {
            get { return tessellation; }
        }

        public Ellipse Ellipse
        {
            get { return ellipse; }
        }

        public FillEllipseInstruction(Ellipse ellipse, int tessellation = Designer.EllipseTessellation)
        {
            this.tessellation = tessellation;
            this.ellipse = ellipse;
        }

        public virtual void Execute(Designer designer)
        {
            designer.FillEllipse(ellipse, tessellation);
        }
    }
}
