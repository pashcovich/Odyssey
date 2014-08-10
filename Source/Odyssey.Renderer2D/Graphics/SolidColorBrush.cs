#region Using Directives

using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct2D1;

#endregion Using Directives

namespace Odyssey.Graphics
{
    internal class SolidColorBrush : Brush
    {
        private readonly SolidColor solidColorBrush;
        private new readonly SharpDX.Direct2D1.SolidColorBrush Resource;

        private SolidColorBrush(string name, Direct2DDevice device, SolidColor solidColorBrush, SharpDX.Direct2D1.SolidColorBrush brush)
            : base(name, device, brush)
        {
            this.solidColorBrush = solidColorBrush;
            Resource = brush;
        }

        public Color4 Color
        {
            get { return solidColorBrush.Color; }
        }

        public static SolidColorBrush New(string name, Direct2DDevice device, SolidColor solidColorBrush)
        {
            return new SolidColorBrush(name, device, solidColorBrush, new SharpDX.Direct2D1.SolidColorBrush(device, solidColorBrush.Color));
        }
    }
}