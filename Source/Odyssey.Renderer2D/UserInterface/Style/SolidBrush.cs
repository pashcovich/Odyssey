#region Using Directives

using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct2D1;

#endregion Using Directives

namespace Odyssey.UserInterface.Style
{
    internal class SolidBrush : Brush
    {
        private SolidBrush(Direct2DDevice device, SolidColorBrush brush)
            : base(device, brush)
        {
            Initialize(Resource);
        }

        public Color4 Color
        {
            get { return ((SolidColorBrush)Resource).Color; }
        }

        public static SolidBrush New(Direct2DDevice device, Color4 color)
        {
            return new SolidBrush(device, new SolidColorBrush(device, color));
        }
    }
}