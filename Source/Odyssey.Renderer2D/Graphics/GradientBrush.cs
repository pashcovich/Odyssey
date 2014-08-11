using System;
using System.Linq;
using Odyssey.Engine;
using SharpDX.Direct2D1;

namespace Odyssey.Graphics
{
    public abstract class GradientBrush : Brush
    {
        private readonly Gradient gradient;

        protected GradientBrush(string name, Direct2DDevice device, Gradient gradient, SharpDX.Direct2D1.Brush brush)
            : base(name, device, brush)
        {
            this.gradient = gradient;
        }

        public GradientStopCollection GradientStops
        {
            get { return gradient.GradientStops; }
        }

        internal Gradient Gradient { get { return gradient; } }

       
    }
}
