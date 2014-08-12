using System.Linq;
using Odyssey.Animations;
using Odyssey.Engine;
using Odyssey.Geometry;
using Odyssey.Utilities.Reflection;
using SharpDX;
using SharpDX.Direct2D1;

namespace Odyssey.Graphics
{
    public sealed class RadialGradientBrush : GradientBrush
    {
        private readonly RadialGradient radialGradient;
        private new readonly SharpDX.Direct2D1.RadialGradientBrush Resource;

        private RadialGradientBrush(string name, Direct2DDevice device, RadialGradient radialGradient, SharpDX.Direct2D1.RadialGradientBrush brush)
            : base(name, device, radialGradient, brush)
        {
            Resource = brush;
            this.radialGradient = radialGradient;
        }

        public Vector2 Center { get { return radialGradient.Center; } }
        public Vector2 OriginOffset { get { return radialGradient.OriginOffset; } }

        [Animatable]
        public float RadiusX
        {
            get { return radialGradient.RadiusX; }
            set
            {
                if (MathHelper.ScalarNearEqual(radialGradient.RadiusX, value))
                    return;
                radialGradient.RadiusX = value;
                Resource.RadiusX = value;
            }
        }

        [Animatable]
        public float RadiusY
        {
            get { return radialGradient.RadiusY; }
            set
            {
                if (MathHelper.ScalarNearEqual(radialGradient.RadiusY, value))
                    return;
                radialGradient.RadiusY = value;
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
                var brush = new SharpDX.Direct2D1.RadialGradientBrush(device, rgbProperties, d2dGradientStopCollection);
                return new RadialGradientBrush(name, device, radialGradient, brush);
            }
            
        }
    }
}
