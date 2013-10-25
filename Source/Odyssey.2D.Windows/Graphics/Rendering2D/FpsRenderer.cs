using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System.Diagnostics;

namespace Odyssey.Graphics.Rendering2D
{
    public class FpsRenderer : Renderer
    {
        FpsTimer timer;
        TextFormat textFormat;
        Brush sceneColorBrush;

        /// <summary>
        /// Initializes a new instance of <see cref="FpsRenderer"/> class.
        /// </summary>
        public FpsRenderer(FpsTimer timer)
        {
            this.timer = timer;
            Show = true;
        }

        public bool Show { get; set; }

        protected override void OnInitialize(InitializeDirectXEventArgs e)
        {
            sceneColorBrush = ToDispose(new SolidColorBrush(e.DirectX.Direct2D.Context, Color.White));
            textFormat = ToDispose(new TextFormat(e.DirectX.DirectWrite.Factory, "Calibri", 20) { TextAlignment = TextAlignment.Leading, ParagraphAlignment = ParagraphAlignment.Center });
            e.DirectX.Direct2D.Context.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Grayscale;
        }

        protected override void OnRender(IDirectXTarget target)
        {
            if (!Show)
                return;

            var context2D = target.Direct2D.Context;
            
            context2D.Transform = SharpDX.Matrix.Identity;
            context2D.DrawText(string.Format("{0:F2} FPS ({1:F1} ms)", timer.MeasuredFPS, timer.FrameTime * 1000.0), textFormat, new RectangleF(8, 8, 8 + 256, 8 + 16), sceneColorBrush);

        }



    }
}
