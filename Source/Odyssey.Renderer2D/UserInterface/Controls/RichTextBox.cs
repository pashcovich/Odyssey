using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Reflection;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls
{
    public class RichTextBox : ItemsControl
    {
        private int lineHeight;

        public RichTextBox() : this(typeof(RichTextBox).Name) { }

        protected RichTextBox(string controlStyleClass, string textStyleClass = UserInterface.Style.TextStyle.Default)
            : base(controlStyleClass, textStyleClass)
        { }

        public IEnumerable<Label> Blocks { get { return Controls.OfType<Label>(); } }

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

        protected override DataTemplate CreateDefaultTemplate()
        {
            string typeName = GetType().Name;
            DataTemplate = new DataTemplate
            {
                Key = string.Format("{0}.TemplateInternal", typeName),
                DataType = GetType(),
                VisualTree = new Label()
                {
                    Name = string.Format("{0}TextBlock", typeName),
                    Width = Width - Padding.Horizontal,
                    Height = LineHeight,
                    TextStyleClass = TextStyleClass
                }
            };
            DataTemplate.Bindings.Add(ReflectionHelper.GetPropertyName((Label l) => l.Text), new Binding(DataTemplate.VisualTree.Name, string.Empty));
            return DataTemplate;
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            if (lineHeight == 0)
                LineHeight = TextStyle.Size;
            if (DataTemplate != null)
                ((Control)DataTemplate.VisualTree).TextStyleClass = TextStyleClass;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            LayoutManager.DistributeVertically(availableSizeWithoutMargins, Controls);
            return base.ArrangeOverride(availableSizeWithoutMargins);
        }
    }
}
