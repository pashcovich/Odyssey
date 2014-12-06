#region Using Directives

using System;
using System.Diagnostics.Contracts;
using Odyssey.Reflection;
using Odyssey.UserInterface.Data;
using Odyssey.Logging;
using SharpDX.Mathematics;
using System.Collections;
using System.Linq;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public abstract class ItemsControl : Control
    {
        private Panel container;
        private IEnumerable itemsSource;

        protected ItemsControl(string controlStyleClass, string textStyleClass = UserInterface.Style.TextStyle.Default)
            : base(controlStyleClass, textStyleClass) {}

        protected Panel Container { get { return container; } }

        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate ItemsPanelTemplate { get; set; }

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

        protected virtual DataTemplate CreateDefaultItemTemplate()
        {
            string typeName = GetType().Name;
            var itemTemplate = new DataTemplate
            {
                Key = string.Format("{0}.ItemTemplate", typeName),
                DataType = GetType(),
                VisualTree = new TextBlock
                {
                    Name = string.Format("{0}TextBlock", typeName),
                    TextStyleClass = TextStyleClass
                }
            };
            itemTemplate.Bindings.Add(ReflectionHelper.GetPropertyName((TextBlock l) => l.Text), new Binding(itemTemplate.VisualTree.Name, string.Empty));
            return itemTemplate;
        }

        protected virtual DataTemplate CreateDefaultPanelTemplate()
        {
            string typeName = GetType().Name;
            var panelTemplate = new DataTemplate()
            {
                Key = string.Format("{0}.PanelTemplate", typeName),
                DataType = GetType(),
                VisualTree  = new StackPanel() {Orientation = Orientation.Vertical}
            };
            return panelTemplate;
        }

        protected override void OnDataContextChanged(EventArgs e)
        {
            if (ItemsPanelTemplate == null)
                ItemsPanelTemplate = CreateDefaultPanelTemplate();
            if (ItemTemplate == null)
                ItemTemplate = CreateDefaultItemTemplate();

            CreateContainer();
            PopulateItems();
        }

        void CreateContainer()
        {
            Contract.Requires<InvalidOperationException>(ItemTemplate.VisualTree is Panel, "VisualTree root must be a Panel");
            var panel = (Panel)ToDispose(ItemsPanelTemplate.VisualTree.Copy());
            if (panel.IsItemsHost)
                container = panel;
            else
            {
                foreach (var child in TreeTraversal.PreOrderVisit(panel))
                {
                    var childPanel = child as Panel;

                    if (childPanel != null && childPanel.IsItemsHost)
                    {
                        container = childPanel;
                        break;
                    }
                }
            }

            foreach (var kvp in ItemsPanelTemplate.Bindings)
            {
                var target = panel.Find(kvp.Value.TargetElement);
                if (target != null)
                {
                    target.DataContext = DataContext;
                    target.SetBinding(kvp.Value, kvp.Key);
                    break;
                }
            }

            if (container == null)
                container = panel;

            Controls.Add(panel);
        }

        void PopulateItems()
        {
            if (ItemsSource == null)
                return;

            if (ItemTemplate.DataType != null && !ReflectionHelper.IsTypeDerived(ItemTemplate.DataType, GetType()))
            {
                LogEvent.UserInterface.Warning(string.Format("ItemTemplate '{0}' expects type {1}. Element '{2}' is of type {3}",
                    ItemTemplate.Key, ItemTemplate.DataType.Name, Name, GetType().Name));
                return;
            }

            int itemCount = 1;
            foreach (var item in ItemsSource)
            {
                UIElement previousElement = this;
                foreach (UIElement element in TreeTraversal.PreOrderVisit(ItemTemplate.VisualTree))
                {
                    UIElement newItem = ToDispose(element.Copy());

                    if (element.Parent is ContentControl)
                        ((ContentControl)previousElement).Content = newItem;
                    else
                        container.Add(newItem);

                    newItem.DataContext = item;

                    foreach (var kvp in ItemTemplate.Bindings.Where(kvp => string.Equals(newItem.Name, kvp.Value.TargetElement)))
                        newItem.SetBinding(kvp.Value, kvp.Key);

                    newItem.Name = string.Format("{0}{1:D2}", newItem.Name, itemCount++);
                    previousElement = newItem;
                }
            }
        }

        #region IContainer members

        public override void Render()
        {
            foreach (UIElement control in Controls.Where(control => control.IsVisible))
                control.Render();
        }

        #endregion IContainer members
    }
}