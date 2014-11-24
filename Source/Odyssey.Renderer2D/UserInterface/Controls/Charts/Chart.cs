using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls.Charts
{
    public abstract class Chart : ItemsControl
    {
        private Control xAxisTitle;

        protected Chart(string controlStyleClass, string textStyleClass = TextStyle.Default)
            : base(controlStyleClass, textStyleClass)
        {
        }

        protected IEnumerable<ChartItem> Items
        {
            get { return Controls.OfType<ChartItem>(); }
        }

        public float MinimumValue { get; set; }
        public float MaximumValue { get; set; }

        public Control XAxisTitle
        {
            get { return xAxisTitle; }
            set
            {
                if (xAxisTitle == value)
                    return;
                Remove(xAxisTitle);
                xAxisTitle = value;
                xAxisTitle.IsInternal = true;
                
                xAxisTitle.Width = Width;
                xAxisTitle.Height = LayoutManager.Units(1);
                xAxisTitle.Position = new Vector2(0, Height - Padding.Bottom - XAxisTitle.Height);
                Add(xAxisTitle);
                Layout(RenderSize);
            }
        }

        public override float ClientAreaHeight
        {
            get
            {
                float areaHeight = base.ClientAreaHeight;
                if (xAxisTitle != null)
                    areaHeight -= xAxisTitle.Height;
                return areaHeight;
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            foreach (var item in Controls.OfType<ChartItem>())
                item.Chart = this;
            base.OnInitialized(e);
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            foreach (var control in Controls.OfType<Control>())
                control.TextStyleClass = TextStyleClass;
        }
    }
}
