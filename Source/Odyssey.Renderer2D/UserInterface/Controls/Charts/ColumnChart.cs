using System;
using System.Collections;
using Odyssey.Reflection;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

namespace Odyssey.UserInterface.Controls.Charts
{
    public class ColumnChart : Chart
    {
        private readonly StackPanel chartArea;

        public ColumnChart()
            : this("Panel", TextStyle.Default)
        {
        }

        public ColumnChart(string controlStyleClass, string textStyleClass) : base(controlStyleClass, textStyleClass)
        {
            chartArea = new StackPanel
            {
                DataTemplate = new DataTemplate
                {
                    Key = string.Format("{0}.TemplateInternal", GetType().Name),
                    DataType = typeof(StackPanel),
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


        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            AddColumnDefinition(new StripDefinition(StripType.Fixed, 1));
            AddRowDefinition(new StripDefinition(StripType.Fixed, 4));
            AddRowDefinition(new StripDefinition(StripType.Fixed, 1));
            chartArea.DependencyProperties.Add(RowPropertyKey, 0);
            XAxisTitle.DependencyProperties.Add(RowPropertyKey, 1);

            chartArea.ItemsSource = ItemsSource;

            Add(chartArea);
        }

        //protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        //{
        //    LayoutManager.DistributeHorizontally(availableSizeWithoutMargins, Controls.Public);
        //    LayoutManager.AlignBottom(this, Controls.Public);
        //    return base.ArrangeOverride(availableSizeWithoutMargins);
        //}
    }
}
