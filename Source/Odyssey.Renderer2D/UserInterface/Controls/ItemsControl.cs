#region License

// Copyright © 2013-2014 Iter Astris - Adalberto L. Simeone
// Web: http://www.iterastris.uk E-mail: adal@iterastris.uk
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion

#region Using Directives

using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Reflection;
using Odyssey.Text.Logging;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public abstract class ItemsControl : Control
    {
        private Panel container;
        private IEnumerable itemsSource;

        protected ItemsControl(string controlStyleClass, string textStyleClass = TextStyle.Default)
            : base(controlStyleClass, textStyleClass)
        {
        }

        protected Panel Container
        {
            get { return container; }
        }

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
            var panelTemplate = new DataTemplate
            {
                Key = string.Format("{0}.PanelTemplate", typeName),
                DataType = GetType(),
                VisualTree = new StackPanel {Orientation = Orientation.Vertical}
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

        private void CreateContainer()
        {
            Contract.Requires<InvalidOperationException>(ItemTemplate.VisualTree is Panel, "VisualTree root must be a Panel");
            var panel = (Panel) ToDispose(ItemsPanelTemplate.VisualTree.Copy());
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

            SetParent(panel, this);
        }

        private void PopulateItems()
        {
            if (ItemsSource == null)
                return;

            if (ItemTemplate.DataType != null && !ReflectionHelper.IsTypeDerived(ItemTemplate.DataType, GetType()))
            {
                LogEvent.UserInterface.Warning(string.Format("ItemTemplate '{0}' expects type {1}. Element '{2}' is of type {3}",
                    ItemTemplate.Key, ItemTemplate.DataType.Name, Name, GetType().Name));
                return;
            }

            foreach (var item in ItemsSource)
            {
                UIElement previousElement = container;
                TemplateItem(previousElement, item, ItemTemplate.VisualTree);
            }
        }

        void TemplateItem(UIElement container, object dataContext, UIElement templateRoot)
        {
            foreach (UIElement element in TreeTraversal.PreOrderVisit(templateRoot))
            {
                UIElement instanceElement = ToDispose(element.Copy());
                var control = container as ContentControl;
                if (control != null)
                    control.Content = instanceElement;
                else
                    SetParent(instanceElement, container);

                instanceElement.DataContext = dataContext;

                foreach (var kvp in ItemTemplate.Bindings)
                {
                    if (string.Equals(instanceElement.Name, kvp.Value.TargetElement))
                        instanceElement.SetBinding(kvp.Value, kvp.Key);
                    //else
                    //    LogEvent.UserInterface.Error("TargetElement {0} not found in {1}", kvp.Value.TargetElement, newItem.Name);
                }
                instanceElement.Name = string.Format("{0}{1:D2}", instanceElement.Name, container.Children.Count()+1);
                container = instanceElement;
            }
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            Container.Measure(availableSizeWithoutMargins + MarginInternal);
            return Container.DesiredSize;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            Container.Arrange(availableSizeWithoutMargins + MarginInternal);
            return availableSizeWithoutMargins;
        }
    }
}