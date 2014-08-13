using System;
using System.Linq;
using Odyssey.Engine;
using SharpDX.Direct2D1;

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

<<<<<<< HEAD
        internal Gradient Gradient { get { return ColorResource; } }
=======
        internal Gradient Gradient { get { return gradient; } }

       
>>>>>>> f72d6706922fc28ded402ed2e8adac271a46849a
    }
}
