using Odyssey.Engine;
using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Style;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Brush = Odyssey.UserInterface.Style.Brush;
using ParagraphAlignment = SharpDX.DirectWrite.ParagraphAlignment;
using TextAlignment = SharpDX.DirectWrite.TextAlignment;

namespace Odyssey.UserInterface.Controls
{
    public class FpsCounter : Label
    {
        private readonly FpsTimer timer;
        private Brush sceneColorBrush;

        public FpsCounter()
        {
            timer = new FpsTimer();
        }

        public override void Render()
        {
            if (!IsVisible)
                return;

            timer.Measure();
            Device.Transform = Matrix.Identity;
            Device.DrawTest(string.Format("{0:F2} FPS ({1:F1} ms)", timer.MeasuredFPS, timer.FrameTime * 1000.0), TextFormat, new RectangleF(8, 8, 8 + 256, 8 + 16), sceneColorBrush);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            sceneColorBrush = ToDispose(SolidBrush.New(Device, Color.White));
            TextFormat = ToDispose(new TextFormat(Device, "Calibri", 20) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center });
            Device.SetTextAntialias(AntialiasMode.Aliased);
        }
    }
}