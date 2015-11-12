#region Using Directives

using Odyssey.Animations;
using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct2D1;

#endregion Using Directives

namespace Odyssey.Graphics
{
    public sealed class SolidColorBrush : Brush
    {
        private readonly SolidColor colorResource;
        private readonly SharpDX.Direct2D1.SolidColorBrush resource;

        private SolidColorBrush(string name, Direct2DDevice device, SolidColor solidColor, SharpDX.Direct2D1.SolidColorBrush brush)
            : base(name, device, solidColor, brush)
        {
            colorResource = solidColor;
            resource = brush;
        }

        [Animatable]
        public Color4 Color
        {
            get { return colorResource.Color; }
            set
            {
                if (colorResource.Color == value)
                    return;
                colorResource.Color = value;
                resource.Color = value;
            }
        }

        public static SolidColorBrush New(string name, Direct2DDevice device, SolidColor solidColorBrush)
        {
            var brushProperties = new BrushProperties { Opacity = solidColorBrush.Opacity };
            return new SolidColorBrush(name, device, solidColorBrush, new SharpDX.Direct2D1.SolidColorBrush(device, solidColorBrush.Color, brushProperties));
        }
    }
}