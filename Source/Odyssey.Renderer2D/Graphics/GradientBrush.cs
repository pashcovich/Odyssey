using Odyssey.Engine;

namespace Odyssey.Graphics
{
    public abstract class GradientBrush : Brush
    {
        protected new readonly Gradient ColorResource;

        protected GradientBrush(string name, Direct2DDevice device, Gradient gradient, SharpDX.Direct2D1.Brush brush)
            : base(name, device, gradient,brush)
        {
            ColorResource = gradient;
        }

        public GradientStopCollection GradientStops
        {
            get { return ColorResource.GradientStops; }
        }

        internal Gradient Gradient { get { return ColorResource; } }
    }
}
