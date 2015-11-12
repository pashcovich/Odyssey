using System;
using System.Diagnostics.Contracts;
using Odyssey.Graphics;
using Odyssey.UserInterface.Style;
using SharpDX;
using SharpDX.DirectWrite;
using TextRenderer = Odyssey.Graphics.TextRenderer;

namespace Odyssey.UserInterface.Controls
{
    public class Label : TextBlockBase
    {
        private TextRenderer textRenderer;

        public Brush Background { get; set; }

        public Label()
            : base(DefaultTextClass)
        {
        }

        public override void Render()
        {
            TextLayout.Draw(textRenderer, AbsolutePosition.X, AbsolutePosition.Y);
        }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            var styleService = Overlay.Services.GetService<IStyleService>();
            if (Background == null)
            {
                var color = new SolidColor(string.Format("{0}.BackgroundFill", Name), Color.Transparent);
                Background = styleService.GetBrushResource(color);
            }
        }

        public void SetColor(int start, int length, Brush brush)
        {
            Contract.Requires<ArgumentNullException>(brush != null, "brush");
            TextLayout.SetDrawingEffect(brush, new TextRange(start, length));
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            textRenderer = new TextRenderer(Device, Foreground);
        }
    }
}
