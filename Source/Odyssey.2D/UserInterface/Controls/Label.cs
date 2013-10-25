using Odyssey.Engine;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public class Label : Control, IRenderable
    {
        private const string ControlTag = "Label";
        private static int count;
        private string text;

        private SolidColorBrush textBrush;
        private TextFormat textFormat;
        private RectangleF layoutRectangle;

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                    text = value;
            }
        }

        public Label()
            : base(ControlTag + ++count, "Empty")
        {
            CanRaiseEvents = false;
        }

        public void Initialize(IDirectXProvider directX)
        {
            IDirect2DProvider direct2D = directX.Direct2D;
            textBrush = ToDispose(new SolidColorBrush(direct2D.Context, TextDescription.Color));
            textFormat = ToDispose(TextDescription.ToTextFormat(directX.DirectWrite));
            direct2D.Context.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Grayscale;
            if (string.IsNullOrEmpty(text))
                text = Id;
        }

        void UpdateLayoutRectangle(RectangleF parentRectangle)
        {
            layoutRectangle = new RectangleF(parentRectangle.X + Position.X, parentRectangle.Y + Position.Y,
                parentRectangle.Width - Position.X, parentRectangle.Height - Position.Y);
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            UpdateLayoutRectangle((RectangleF)Parent);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateLayoutRectangle((RectangleF)Parent);
        }

        protected override void OnPositionChanged(EventArgs e)
        {
            base.OnPositionChanged(e);
            UpdateLayoutRectangle((RectangleF)Parent);
        }

        public void Render(Engine.IDirectXTarget target)
        {
            target.Direct2D.Context.DrawText(Text, textFormat, layoutRectangle, textBrush);
        }

        public override bool IntersectTest(Vector2 cursorLocation)
        {
            return layoutRectangle.Contains(cursorLocation);
        }
    }
}
