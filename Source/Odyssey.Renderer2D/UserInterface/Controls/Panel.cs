#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
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
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.UserInterface.Events;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public abstract class Panel : UIElement, IContainer
    {
        protected Panel()
        {
            IsFocusable = false;
        }

        #region IContainer Members

        /// <summary>
        /// Occurs when a new control is added to the <see cref="UIElementCollection"/>.
        /// </summary>
        public event EventHandler<UIElementEventArgs> LogicalChildAdded;

        public bool IsItemsHost { get; set; }

        /// <summary>
        /// Raises the <see cref="LogicalChildAdded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance
        /// containing the event data.</param>
        protected virtual void OnLogicalChildAdded(UIElementEventArgs e)
        {
            RaiseEvent(LogicalChildAdded, this, e);
            if (!DesignMode)
                Layout(RenderSize);
        }

        #endregion IContainer Members

        public override UIElement Parent
        {
            get { return base.Parent; }
            internal set
            {
                if (base.Parent == value) return;

                base.Parent = value;
                foreach (UIElement control in Children)
                    control.Parent = this;
            }
        }

        public void Add(UIElement item)
        {
            SetParent(ToDispose(item), this);
            OnLogicalChildAdded(new UIElementEventArgs(item, Children.Count - 1));
        }
      
        public void AddRange(IEnumerable<UIElement> controls)
        {
            foreach (UIElement ctl in controls)
                Add(ctl);
        }

        /// <summary>
        /// Determines whether the <b>ContainerControl</b> contains the specified Key.
        /// </summary>
        /// <param name="control">The control to locate in the control collection.</param>
        /// <returns><b>True</b> if it the collection contains that element ,<b>false</b>
        /// otherwise.</returns>
        /// <remarks>
        /// The control passed as parameter does not have to be a top level child, but this method
        /// will also return true if the specified <see cref="UIElement"/> belongs to the tree
        /// formed by the ContainerControl's children.
        /// </remarks>
        public bool ContainsControl(UIElement control)
        {
            Contract.Requires<ArgumentNullException>(control != null, "control is null");

            return Children.Contains(control);
        }

        public bool ContainsControl(string name)
        {
            return Children.Any(c => string.Equals(c.Name, name));
        }

        public UIElement Find(string id)
        {
            foreach (UIElement ctl in TreeTraversal.PreOrderVisit(this).Skip(1))
            {
                if (ctl.Name == id)
                    return ctl;
            }
            return null;
        }

        public UIElement Find(Vector2 cursorLocation)
        {
            return TreeTraversal.PostOrderInteractionVisit(this)
                .Reverse()
                .FirstOrDefault(control => control.Contains(cursorLocation));
        }

        public void Remove(UIElement item)
        {
            Children.Remove(item);
        }

        public override void Render()
        {
            foreach (UIElement control in Children)
            {
                if (control.IsVisible)
                    control.Render();
            }
        }

        protected internal override UIElement Copy()
        {
            UIElement copy = base.Copy();
            CopyEvents(typeof(Panel), this, copy);
            var containerCopy = (Panel) copy;
            containerCopy.IsItemsHost = IsItemsHost;
            foreach (UIElement child in Children)
                containerCopy.Add(child.Copy());

            return copy;
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            var requiredSize = DesiredSize;
            foreach (UIElement ctl in Children)
            {
                ctl.Measure(availableSizeWithoutMargins);
            }
            return requiredSize;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            foreach (UIElement ctl in Children)
            {
                ctl.Arrange(availableSizeWithoutMargins);
            }

            return availableSizeWithoutMargins;
        }

        #region Debug

#if DEBUG
        internal static void Debug(IEnumerable<UIElement> iterator)
        {
            System.Diagnostics.Debug.WriteLine("---------");
            foreach (UIElement ctl in iterator)
            {
                System.Diagnostics.Debug.WriteLine(ctl.Name);
            }
            System.Diagnostics.Debug.WriteLine("---------");
        }
#endif

        #endregion Debug
    }
}