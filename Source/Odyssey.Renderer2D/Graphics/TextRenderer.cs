#region Using Directives

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.Graphics
{
    public class TextRenderer : TextRendererBase
    {
        private readonly Brush defaultBrush;
        private readonly RenderTarget renderTarget;

        public TextRenderer(RenderTarget renderTarget, Brush defaultBrush)
        {
            this.renderTarget = renderTarget;
            this.defaultBrush = defaultBrush;
        }

        public override Result DrawGlyphRun(object clientDrawingContext, float baselineOriginX, float baselineOriginY, MeasuringMode measuringMode,
            GlyphRun glyphRun, GlyphRunDescription glyphRunDescription, ComObject clientDrawingEffect)
        {
            SharpDX.Direct2D1.Brush brush = defaultBrush;
            if (clientDrawingEffect != null && clientDrawingEffect is SharpDX.Direct2D1.Brush)
            {
                brush = (SharpDX.Direct2D1.Brush) clientDrawingEffect;
            }

            try
            {
                renderTarget.DrawGlyphRun(new Vector2(baselineOriginX, baselineOriginY), glyphRun, brush, measuringMode);
                return Result.Ok;
            }
            catch
            {
                return Result.Fail;
            }
        }
    }
}