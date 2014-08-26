using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Graphics;
using SharpDX;

namespace Odyssey.UserInterface.Controls
{
    public class RichTextBox : PanelBase
    {
        private int lineHeight;

        public RichTextBox() : base(typeof(RichTextBox).Name) { }

        protected RichTextBox(string controlClass)
            : base(controlClass)
        {}

        public IEnumerable<AdvancedLabel> Blocks { get { return Controls.OfType<AdvancedLabel>(); } }

        public int LineHeight
        {
            get { return lineHeight; }
            set
            {
                if (lineHeight == value)
                    return;
                lineHeight = value;
                foreach (var block in Blocks)
                {
                    block.Height = lineHeight;
                }
            }
        }

        protected override void OnTextDefinitionChanged(EventArgs e)
        {
            base.OnTextDefinitionChanged(e);
            if (lineHeight == 0)
                LineHeight = TextDescription.Size;
        }

        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        protected override void Arrange()
        {
            base.Arrange();
            for (int i = 0; i < Controls.Count; i++)
            {
                var block = Controls[i];
                block.Position = new Vector2(Padding.Left, i* block.Height);
            }
        }

        public virtual void AddBlock(string text)
        {
            Add(ToDispose(new AdvancedLabel()
            {
                Width = Width - Padding.Horizontal,
                Height = LineHeight,
                TextStyleClass = TextStyleClass,
                Text = text
            }));
        }
    }
}
