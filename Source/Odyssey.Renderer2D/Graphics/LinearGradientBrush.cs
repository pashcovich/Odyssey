using System.Linq;
using Odyssey.Engine;
using SharpDX.Mathematics;
using SharpDX.Direct2D1;

namespace Odyssey.Graphics
{
    public sealed class LinearGradientBrush : GradientBrush
    {
        private new readonly LinearGradient colorResource;
        private new readonly SharpDX.Direct2D1.LinearGradientBrush resource;

        private LinearGradientBrush(string name, Direct2DDevice device, LinearGradient linearGradient, SharpDX.Direct2D1.LinearGradientBrush brush)
            : base(name, device, linearGradient, brush)
        {
            resource = brush;
            colorResource = linearGradient;
        }

        public Vector2 StartPoint { get { return colorResource.StartPoint; } }
        public Vector2 EndPoint { get { return colorResource.EndPoint; } }

        public static LinearGradientBrush New(string name, Direct2DDevice device, LinearGradient linearGradient)
        {
           
            using (var d2dGradientStopCollection = new SharpDX.Direct2D1.GradientStopCollection(device,
                linearGradient.GradientStops.Select(gs => (SharpDX.Direct2D1.GradientStop) gs).ToArray(),
                (SharpDX.Direct2D1.ExtendMode) linearGradient.GradientStops.ExtendMode))
            {
                var lgbProperties = new LinearGradientBrushProperties() { StartPoint = linearGradient.StartPoint, EndPoint = linearGradient.EndPoint };
                var brushProperties = new BrushProperties() { Opacity = linearGradient.Opacity };
                var brush = new SharpDX.Direct2D1.LinearGradientBrush(device, lgbProperties, brushProperties, d2dGradientStopCollection);
                return new LinearGradientBrush(name, device, linearGradient, brush); 
            }
            
        }
    }
}
