using Odyssey.Graphics;
using Odyssey.Graphics.Rendering2D;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public abstract class LabelBase : Control, IRenderable
    {
        private const string ControlTag = "Label";
        private static int count;
        private string text;

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

        public LabelBase()
            : base(ControlTag + ++count, "Empty")
        {
            CanRaiseEvents = false;
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



        public override bool Contains(Vector2 cursorLocation)
        {
            return layoutRectangle.Contains(cursorLocation);
        }
    }
}
