using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls.Charts
{
    public abstract class Chart : Grid
    {
        private Control xAxisTitle;

        private IEnumerable itemsSource;

        public IEnumerable ItemsSource
        {
            get { return itemsSource; }
            set
            {
                if (!Equals(itemsSource, value))
                {
                    itemsSource = value;
                    DataContext = value;
                }
            }
        }

        protected Chart(string controlStyleClass, string textStyleClass = TextStyle.Default)
            : base()
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
                Add(xAxisTitle);
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

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            foreach (var control in Controls.OfType<Control>())
                control.TextStyleClass = TextStyleClass;
        }
    }
}
