using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics.Shapes;
using SharpDX.Direct2D1;
using GradientStopCollection = Odyssey.Graphics.Shapes.GradientStopCollection;

namespace Odyssey.UserInterface.Style
{
    internal class LinearGradientBrush : Brush
    {
        private readonly LinearGradient linearGradient;

        private LinearGradientBrush(string name, Direct2DDevice device, LinearGradient linearGradient, LinearGradientBrushProperties lgbProperties, SharpDX.Direct2D1.GradientStopCollection gsCollection)
            : base(name, device, new SharpDX.Direct2D1.LinearGradientBrush(device, lgbProperties, gsCollection))
        {
            this.linearGradient = linearGradient;
            ToDispose(gsCollection);
            
        }

        public GradientStopCollection GradientStops
        {
            get { return linearGradient.GradientStops; }
        }

        public static LinearGradientBrush New(string name, Direct2DDevice device, LinearGradient gradient)
        {
            var lgbProperties = new LinearGradientBrushProperties() { StartPoint = gradient.StartPoint, EndPoint = gradient.EndPoint };
            var d2dGradientStopCollection = new SharpDX.Direct2D1.GradientStopCollection(device,
                gradient.GradientStops.Select(gs => (SharpDX.Direct2D1.GradientStop) gs).ToArray(), (SharpDX.Direct2D1.ExtendMode)gradient.GradientStops.ExtendMode);

            return new LinearGradientBrush(name, device, gradient,lgbProperties, d2dGradientStopCollection);
        }
    }
}
