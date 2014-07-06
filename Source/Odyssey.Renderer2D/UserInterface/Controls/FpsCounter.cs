using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using Odyssey.Graphics.Shapes;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Brush = Odyssey.Graphics.Shapes.Brush;

namespace Odyssey.UserInterface.Controls
{
    public class FpsCounter : Label
    {
        Brush sceneColorBrush;
        private readonly FpsTimer timer;

        public FpsCounter() : base("Label")
        {
            timer = new FpsTimer();
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            sceneColorBrush = ToDispose(SolidBrush.New(Device, Color.White));
            TextFormat = ToDispose(new TextFormat(Device, "Calibri", 20) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center });
            Device.SetTextAntialias(AntialiasMode.Aliased);
            
    
        }

        public override void Render()
        {
            if (!IsVisible)
                return;

            timer.Measure();
            Device.SetTransform(Matrix.Identity);
            Device.DrawTest(string.Format("{0:F2} FPS ({1:F1} ms)", timer.MeasuredFPS, timer.FrameTime * 1000.0), TextFormat, new RectangleF(8, 8, 8 + 256, 8 + 16), sceneColorBrush);

        }

    }
}
