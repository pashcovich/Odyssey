using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Reflection;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Controls.Charts
{
    public class ColumnChart : Chart
    {
        public ColumnChart()
            : base(ControlStyle.Empty, TextStyle.Default)
        {
        }

        protected IEnumerable<ChartItem> Items
        {
            get { return ChartArea.Controls.OfType<ChartItem>(); }
        }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            var dockpanel = FindDescendants<DockPanel>().First();
            XAxisTitle = dockpanel.Controls[IdXAxisTitle];
            XAxisTitle.DependencyProperties.Add(DockPanel.DockPropertyKey, Dock.Bottom);
            ChartArea = dockpanel.Controls[IdChartArea];
            //ChartArea.StyleClass = StyleClass;
            //ChartArea.ItemsSource = ItemsSource;
        }

        protected override DataTemplate CreateDefaultItemTemplate()
        {
            var itemTemplate = new DataTemplate
            {
                Key = string.Format("{0}.TemplateInternal", GetType().Name),
                DataType = GetType(),
                VisualTree = new ColumnItem()
                {
                    Name = typeof (ColumnItem).Name,
                    Margin = new Thickness(0, 0, 4, 0),
                    VerticalAlignment = VerticalAlignment.Bottom
                }
            };
            itemTemplate.Bindings.Add(ReflectionHelper.GetPropertyName((ChartItem c) => c.Value), new Binding(itemTemplate.VisualTree.Name, string.Empty));

            return itemTemplate;
        }

        protected override DataTemplate CreateDefaultPanelTemplate()
        {
            string typeName = GetType().Name;
            var panelTemplate = new DataTemplate()
            {
                Key = string.Format("{0}.PanelTemplate", typeName),
                DataType = GetType(),
                VisualTree = new DockPanel()
                {
                    new Label() {Name = IdXAxisTitle, TextStyleClass = TextStyle.TemplatedParent},
                    new UniformStackPanel {Name = IdChartArea, IsItemsHost = true}
                }
            };
            panelTemplate.Bindings.Add((ReflectionHelper.GetPropertyName((TextBlock t) => t.Text)), new Binding(IdXAxisTitle, IdXAxisTitle));
            return panelTemplate;
        }

    }
}
