#region Using Directives

using System.Linq;
using Odyssey.Animations;
using Odyssey.Engine;
using Odyssey.Geometry;
using SharpDX.Direct2D1;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.Graphics
{
    public sealed class RadialGradientBrush : GradientBrush
    {
        private readonly RadialGradient colorResource;
        private readonly SharpDX.Direct2D1.RadialGradientBrush resource;

        private RadialGradientBrush(string name, Direct2DDevice device, RadialGradient radialGradient,
            SharpDX.Direct2D1.RadialGradientBrush brush)
            : base(name, device, radialGradient, brush)
        {
            resource = brush;
            colorResource = radialGradient;
        }

        public Vector2 Center
        {
            get { return colorResource.Center; }
        }

        public Vector2 OriginOffset
        {
            get { return colorResource.OriginOffset; }
        }

        [Animatable]
        public float RadiusX
        {
            get { return colorResource.RadiusX; }
            set
            {
                if (MathHelper.ScalarNearEqual(colorResource.RadiusX, value))
                    return;
                colorResource.RadiusX = value;
                resource.RadiusX = value;
            }
        }

        [Animatable]
        public float RadiusY
        {
            get { return colorResource.RadiusY; }
            set
            {
                if (MathHelper.ScalarNearEqual(colorResource.RadiusY, value))
                    return;
                colorResource.RadiusY = value;
                resource.RadiusY = value;
            }
        }

        public static RadialGradientBrush New(string name, Direct2DDevice device, RadialGradient radialGradient)
        {
            using (var d2dGradientStopCollection = new SharpDX.Direct2D1.GradientStopCollection(device,
                radialGradient.GradientStops.Select(gs => (SharpDX.Direct2D1.GradientStop) gs).ToArray(),
                (SharpDX.Direct2D1.ExtendMode) radialGradient.GradientStops.ExtendMode))
            {
                var rgbProperties = new RadialGradientBrushProperties
                {
                    Center = radialGradient.Center,
                    GradientOriginOffset = radialGradient.OriginOffset,
                    RadiusX = radialGradient.RadiusX,
                    RadiusY = radialGradient.RadiusY
                };
                var brushProperties = new BrushProperties {Opacity = radialGradient.Opacity};
                var brush = new SharpDX.Direct2D1.RadialGradientBrush(device, rgbProperties, brushProperties, d2dGradientStopCollection);
                return new RadialGradientBrush(name, device, radialGradient, brush);
            }
        }
    }
}