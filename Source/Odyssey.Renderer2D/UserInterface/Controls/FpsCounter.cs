using Odyssey.Engine;
using SharpDX;

namespace Odyssey.UserInterface.Controls
{
    public class FpsCounter : TextBlock
    {
        private readonly FpsTimer timer;

        public FpsCounter()
        {
            timer = new FpsTimer();
            TextStyleClass = "Small";
        }

        public override void Render()
        {
            timer.Measure();
            Device.Transform = Matrix.Identity;
            Device.DrawText(string.Format("{0:F2} FPS ({1:F1} ms)", timer.MeasuredFPS, timer.FrameTime * 1000.0), TextFormat, new RectangleF(8, 8, 8 + 256, 8 + 16), Foreground);
        }

    }
}