using System;
using System.Collections;
using System.Linq;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls.Charts
{
    public abstract class Chart : DockPanel
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

        public abstract Vector3 ChartArea { get; }
        
        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            foreach (var control in Controls.OfType<Control>())
                control.TextStyleClass = TextStyleClass;
        }
    }
}
