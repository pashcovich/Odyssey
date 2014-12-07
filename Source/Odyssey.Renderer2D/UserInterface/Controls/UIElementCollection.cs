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

#endregion License

#region Using Directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Geometry;
using SharpDX.Mathematics;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public class UIElementCollection : Collection<UIElement>, IEnumerable<UIElement>
    {
        public UIElementCollection(UIElement owner)
        {
            Owner = owner;
        }

        public IEnumerable<UIElement> InteractionEnabled
        {
            get
            {
                return this.Where(c => c.IsVisible && c.IsEnabled && c.CanRaiseEvents);
            }
        }

        public IEnumerable<UIElement> Visual
        {
            get { return this.Where(c => !c.IsInternal); }
        }

        internal IEnumerable<UIElement> Internal
        {
            get { return this.Where(c => c.IsInternal); }
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public UIElement Owner { get; private set; }

        private IEnumerable<UIElement> AllControls
        {
            get { return Items; }
        }

        #region IEnumerable<BaseControl> Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return AllControls.GetEnumerator();
        }

        IEnumerator<UIElement> IEnumerable<UIElement>.GetEnumerator()
        {
            return AllControls.GetEnumerator();
        }

        #endregion IEnumerable<BaseControl> Members

        /// <summary>
        /// Returns the index of the first level child control (not recursive) whose Id is the one
        /// specified.
        /// </summary>
        /// <param name="id">The Id of the control to find.</param>
        /// <returns>The 0 based index if found, -1 if not.</returns>
        public int IndexOf(string id)
        {
            for (int i = 0; i < Count; i++)
            {
                UIElement ctl = this[i];
                if (ctl.Name == id)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Sort the control collection from the foremost to the ones in the background.
        /// </summary>
        public void Sort()
        {
            UIElement[] controlArray = ToArray();
            Array.Sort(controlArray);

            Clear();

            for (int i = 0; i < controlArray.Length; i++)
            {
                base.InsertItem(i, controlArray[i]);
            }
        }

        public void Sort<TKey>(Func<UIElement,TKey> keySelector)
        {
            var controlArray = Items.OrderBy(keySelector).ToArray();
            Clear();
            for (int i = 0; i < controlArray.Length; i++)
            {
                base.InsertItem(i, controlArray[i]);
            }
        }

        public UIElement[] ToArray()
        {
            var controlArray = new UIElement[Count];
            for (int i = 0; i < Count; i++)
            {
                controlArray[i] = this[i];
            }
            return controlArray;
        }

        protected override void InsertItem(int index, UIElement item)
        {
            ProcessForInsertion(item);
            base.InsertItem(index, item);
            Sort(e=> e.Position.Z);
        }

        private void ProcessForDeletion(UIElement control)
        {
            //Overlay Overlay = Owner as Overlay;

            //Window window = control as Window;

            //if (Overlay != null && window != null)
            //    Overlay.WindowManager.Remove(window);

            control.CanRaiseEvents = false;
            control.IsBeingRemoved = true;

            var containerControl = control as IContainer;

            if (containerControl != null)
                foreach (UIElement childControl in containerControl.Children)
                {
                    childControl.CanRaiseEvents = false;
                    childControl.IsBeingRemoved = true;
                }
        }

        private void ProcessForInsertion(UIElement control)
        {
            Contract.Requires<ArgumentNullException>(control != null);
            Contract.Requires<InvalidOperationException>(!Contains(control), "Control is already in collection.");

            control.Parent = Owner;
            control.Index = Count;
            if (!control.IsInternal)
                control.SetPosition(new Vector3(control.Position.XY(), control.Position.Z+1));
        }

        protected override void RemoveItem(int index)
        {
            ProcessForDeletion(this[index]);
            base.RemoveItem(index);
        }

        public UIElement this[string name]
        {
            get { return Items.First(c => string.Equals(c.Name, name)); }
        }
    }
}