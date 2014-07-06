using Odyssey.Engine;
using Odyssey.Graphics.Shapes;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using Brush = Odyssey.Graphics.Shapes.Brush;

namespace Odyssey.Graphics
{
    public class FpsRenderer : Rendering2D.Renderer
    {
        private readonly FpsTimer timer;
        private TextFormat textFormat;
        private Brush sceneColorBrush;

        /// <summary>
        /// Initializes a new instance of <see cref="FpsRenderer"/> class.
        /// </summary>
        public FpsRenderer(Direct2DDevice device, FpsTimer timer)
            : base(device)
        {
            this.timer = timer;
            Show = true;
        }

        public bool Show { get; set; }

        public override void Initialize()
        {
            sceneColorBrush = ToDispose(SolidBrush.New(Device, Color.White));
            textFormat = ToDispose(new TextFormat(Device, "Calibri", 20) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center });
            Context.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Grayscale;
        }

        public override void Render()
        {
            if (!Show)
                return;

            Context.Transform = SharpDX.Matrix.Identity;
            Context.DrawText(string.Format("{0:F2} FPS ({1:F1} ms)", timer.MeasuredFPS, timer.FrameTime * 1000.0), textFormat, new RectangleF(8, 8, 8 + 256, 8 + 16), sceneColorBrush);
        }
    }
}