#region Using Directives

using Odyssey.Engine;
using Odyssey.Graphics.Shapes;
using SharpDX;
using SharpDX.Direct2D1;

#endregion Using Directives

namespace Odyssey.UserInterface.Style
{
    internal class SolidBrush : Brush
    {
        private readonly UniformGradient uniformGradient;

        private SolidBrush(string name, Direct2DDevice device, UniformGradient uniformGradient)
            : base(name, device, new SolidColorBrush(device, uniformGradient.Color))
        {
            this.uniformGradient = uniformGradient;
        }

        public Color4 Color
        {
            get { return ((SolidColorBrush)Resource).Color; }
        }

        public static SolidBrush New(string name, Direct2DDevice device, Color4 color)
        {
            return new SolidBrush(name, device, new UniformGradient(name, color));
        }
    }
}