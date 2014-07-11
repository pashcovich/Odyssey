#region Using Directives

using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct2D1;

#endregion Using Directives

namespace Odyssey.Graphics.Shapes
{
    public class SolidBrush : Brush
    {
        private readonly Color4 color;

        private SolidBrush(Direct2DDevice device, Color4 color, SolidColorBrush brush)
            : base(device, brush)
        {
            this.color = color;
            Initialize(Resource);
        }

        public Color4 Color
        {
            get { return color; }
        }

        public static SolidBrush New(Direct2DDevice device, Color4 color)
        {
            return new SolidBrush(device, color, new SolidColorBrush(device, color));
        }
    }
}