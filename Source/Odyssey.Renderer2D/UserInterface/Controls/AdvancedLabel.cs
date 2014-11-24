using System;
using System.Diagnostics.Contracts;
using Odyssey.Graphics;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;
using SharpDX.DirectWrite;
using TextRenderer = Odyssey.Graphics.TextRenderer;

namespace Odyssey.UserInterface.Controls
{
    public class AdvancedLabel : LabelBase
    {
        private TextLayout textLayout;
        private TextRenderer textRenderer;
        private TextMetrics textMetrics;

        public Brush Background { get; set; }

        public AdvancedLabel()
            : base(DefaultTextClass)
        {
        }

        public override void Render()
        {
            textLayout.Draw(textRenderer, AbsolutePosition.X, AbsolutePosition.Y);
        }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            textRenderer = new TextRenderer(Device, Foreground);
            var styleService = Overlay.Services.GetService<IStyleService>();
            if (Background == null)
            {
                var color = new SolidColor(string.Format("{0}.BackgroundFill", Name), Color.Transparent);
                Background = styleService.GetBrushResource(color);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            UpdateTextLayout();
        }

        protected override void OnTextChanged(TextEventArgs e)
        {
            base.OnTextChanged(e);

            if (DesignMode)
                return;
            UpdateTextLayout();
        }

        void UpdateTextLayout()
        {
            if (textLayout != null)
                RemoveAndDispose(ref textLayout);

            textLayout = ToDispose(new TextLayout(Device, Text, TextFormat, Width, Height));
            textMetrics = textLayout.Metrics;
        }

        protected override Vector2 MeasureOverride(Vector2 availableSizeWithoutMargins)
        {
            if (Width == 0)
                Width = textMetrics.Width;
            if (Height == 0)
                Height = textMetrics.Height;

            return base.MeasureOverride(availableSizeWithoutMargins);
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            textLayout.MaxWidth = Width;
            textLayout.MaxHeight = Height;
        }

        public void SetColor(int start, int length, Brush brush)
        {
            Contract.Requires<ArgumentNullException>(brush != null, "brush");
            textLayout.SetDrawingEffect(brush, new TextRange(start, length));
        }

        public float MeasureText()
        {
            return textMetrics.Width;
        }

        public float MeasureLineHeight()
        {
            return textMetrics.Height;
        }

        public float MeasureText(int start, int length)
        {
            var metrics = textLayout.HitTestTextRange(start, length,0, 0);
            return metrics[0].Width;
        }
    }
}
