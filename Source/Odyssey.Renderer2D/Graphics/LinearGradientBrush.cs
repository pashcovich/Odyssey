using System.Linq;
using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct2D1;

namespace Odyssey.Graphics
{
    public sealed class LinearGradientBrush : GradientBrush
    {
        private readonly LinearGradient linearGradient;
        private new readonly SharpDX.Direct2D1.LinearGradientBrush Resource;

        private LinearGradientBrush(string name, Direct2DDevice device, LinearGradient linearGradient, SharpDX.Direct2D1.LinearGradientBrush brush)
            : base(name, device, linearGradient, brush)
        {
            Resource = brush;
            this.linearGradient = linearGradient;
        }

        public Vector2 StartPoint { get { return linearGradient.StartPoint; } }
        public Vector2 EndPoint { get { return linearGradient.EndPoint; } }

        public static LinearGradientBrush New(string name, Direct2DDevice device, LinearGradient linearGradient)
        {
           
            using (var d2dGradientStopCollection = new SharpDX.Direct2D1.GradientStopCollection(device,
                linearGradient.GradientStops.Select(gs => (SharpDX.Direct2D1.GradientStop) gs).ToArray(),
                (SharpDX.Direct2D1.ExtendMode) linearGradient.GradientStops.ExtendMode))
            {
                var lgbProperties = new LinearGradientBrushProperties() { StartPoint = linearGradient.StartPoint, EndPoint = linearGradient.EndPoint };
                var brush = new SharpDX.Direct2D1.LinearGradientBrush(device, lgbProperties, d2dGradientStopCollection);
                return new LinearGradientBrush(name, device, linearGradient, brush);
            }
            
        }
    }
}
