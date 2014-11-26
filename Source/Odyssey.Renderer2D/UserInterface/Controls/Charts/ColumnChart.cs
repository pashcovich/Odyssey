using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Reflection;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls.Charts
{
    public class ColumnChart : Chart
    {
        private readonly UniformStackPanel chartArea;

        public ColumnChart()
            : this("Panel", TextStyle.Default)
        {
        }

        public ColumnChart(string controlStyleClass, string textStyleClass) : base(controlStyleClass, textStyleClass)
        {
            chartArea = new UniformStackPanel
            {
                DataTemplate = new DataTemplate
                {
                    Key = string.Format("{0}.TemplateInternal", GetType().Name),
                    DataType = typeof(UniformStackPanel),
                    VisualTree = new ColumnItem()
                    {
                        Name = typeof (ColumnItem).Name,
                        Margin = new Thickness(0, 0, 4, 0),
                    }
                },
                Orientation = Orientation.Horizontal
            };

            chartArea.DataTemplate.Bindings.Add(ReflectionHelper.GetPropertyName((ChartItem c) => c.Value), new Binding(chartArea.DataTemplate.VisualTree.Name, string.Empty));
        }

        public override Vector3 ChartArea
        {
            get { return chartArea.RenderSize; }
        }

        protected IEnumerable<ChartItem> Items
        {
            get { return chartArea.Controls.OfType<ChartItem>(); }
        }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            XAxisTitle.DependencyProperties.Add(DockPropertyKey, Dock.Bottom);
            chartArea.ItemsSource = ItemsSource;

            Add(chartArea);
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            LayoutManager.DistributeHorizontally(availableSizeWithoutMargins, Controls.Public);
            LayoutManager.AlignBottom(this, Controls.Public);
            return base.ArrangeOverride(availableSizeWithoutMargins);
        }
    }
}
