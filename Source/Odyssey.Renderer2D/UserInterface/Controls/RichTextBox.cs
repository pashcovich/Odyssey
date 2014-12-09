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
using System.Collections.Generic;
using Odyssey.Reflection;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public class RichTextBox : ItemsControl
    {
        private int lineHeight;
        private int lines;

        public RichTextBox() : this(typeof (RichTextBox).Name)
        {
        }

        protected RichTextBox(string controlStyleClass, string textStyleClass = TextStyle.Default)
            : base(controlStyleClass, textStyleClass)
        {
        }

        public IEnumerable<Label> Blocks
        {
            get { return FindDescendants<Label>(); }
        }

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

        protected override DataTemplate CreateDefaultItemTemplate()
        {
            string typeName = GetType().Name;
            var itemTemplate = new DataTemplate
            {
                Key = string.Format("{0}.ItemTemplate", typeName),
                DataType = GetType(),
                VisualTree = new Label
                {
                    Name = string.Format("{0}.TextBlock", typeName),
                    Height = LineHeight,
                    TextStyleClass = TextStyleClass
                }
            };
            itemTemplate.Bindings.Add(ReflectionHelper.GetPropertyName((Label l) => l.Text), new Binding(ItemTemplate.VisualTree.Name, string.Empty));
            return ItemTemplate;
        }

        protected override DataTemplate CreateDefaultPanelTemplate()
        {
            string typeName = GetType().Name;
            var panelTemplate = new DataTemplate
            {
                Key = string.Format("{0}.PanelTemplate", typeName),
                DataType = GetType(),
                VisualTree = new StackPanel
                {
                    Name = string.Format("{0}.{1}", typeName, typeof (StackPanel).Name),
                    Orientation = Orientation.Vertical
                }
            };
            return panelTemplate;
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            if (lineHeight == 0)
                LineHeight = TextStyle.Size;
            if (ItemTemplate != null)
                ((VisualElement) ItemTemplate.VisualTree).TextStyleClass = TextStyleClass;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            LayoutManager.DistributeVertically(availableSizeWithoutMargins, this);
            return availableSizeWithoutMargins;
        }
    }
}