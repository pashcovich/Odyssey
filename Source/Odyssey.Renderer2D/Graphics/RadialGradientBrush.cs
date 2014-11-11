using System.Linq;
using Odyssey.Animations;
using Odyssey.Engine;
using Odyssey.Geometry;
using SharpDX.Mathematics;
using SharpDX.Direct2D1;

namespace Odyssey.Graphics
{
    public sealed class RadialGradientBrush : GradientBrush
    {
        protected new readonly RadialGradient ColorResource;
        private new readonly SharpDX.Direct2D1.RadialGradientBrush Resource;

        private RadialGradientBrush(string name, Direct2DDevice device, RadialGradient radialGradient, SharpDX.Direct2D1.RadialGradientBrush brush)
            : base(name, device, radialGradient, brush)
        {
            Resource = brush;
            ColorResource = radialGradient;
        }

        public Vector2 Center { get { return ColorResource.Center; } }
        public Vector2 OriginOffset { get { return ColorResource.OriginOffset; } }

        [Animatable]
        public float RadiusX
        {
            get { return ColorResource.RadiusX; }
            set
            {
                if (MathHelper.ScalarNearEqual(ColorResource.RadiusX, value))
                    return;
                ColorResource.RadiusX = value;
                Resource.RadiusX = value;
            }
        }

        [Animatable]
        public float RadiusY
        {
            get { return ColorResource.RadiusY; }
            set
            {
                if (MathHelper.ScalarNearEqual(ColorResource.RadiusY, value))
                    return;
                ColorResource.RadiusY = value;
                Resource.RadiusY = value;
            }
        }

        public static RadialGradientBrush New(string name, Direct2DDevice device, RadialGradient radialGradient)
        {
            using (var d2dGradientStopCollection = new SharpDX.Direct2D1.GradientStopCollection(device,
                radialGradient.GradientStops.Select(gs => (SharpDX.Direct2D1.GradientStop)gs).ToArray(),
                (SharpDX.Direct2D1.ExtendMode)radialGradient.GradientStops.ExtendMode))
            {
                var rgbProperties = new RadialGradientBrushProperties
                {
                    Center = radialGradient.Center,
                    GradientOriginOffset = radialGradient.OriginOffset,
                    RadiusX = radialGradient.RadiusX,
                    RadiusY = radialGradient.RadiusY
                };
                var brushProperties = new BrushProperties() {Opacity = radialGradient.Opacity};
                var brush = new SharpDX.Direct2D1.RadialGradientBrush(device, rgbProperties, brushProperties, d2dGradientStopCollection);
                return new RadialGradientBrush(name, device, radialGradient, brush);
            }
            
        }
    }
}
