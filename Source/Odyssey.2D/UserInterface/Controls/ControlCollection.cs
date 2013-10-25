using Odyssey.Engine;
using Odyssey.UserInterface.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace Odyssey.UserInterface.Controls
{
    public class ControlCollection : Collection<UIElement>, IEnumerable<UIElement>
    {
        public ControlCollection(UIElement owner)
        {
            this.Owner = owner;
        }

        public IEnumerable<UIElement> InteractionEnabledControls
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    UIElement control = this[i];
                    if (control.IsVisible && control.IsEnabled)
                        yield return control;
                    else continue;
                }
            }
        }
        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public UIElement Owner { get; private set; }
        internal IEnumerable<UIElement> AllControls
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    yield return this[i];
                }
            }
        }

        #region IEnumerable<BaseControl> Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<UIElement> IEnumerable<UIElement>.GetEnumerator()
        {
            foreach (UIElement control in this)
                yield return control;
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
                if (ctl.Id == id)
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

        public UIElement[] ToArray()
        {
            UIElement[] controlArray = new UIElement[Count];
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
        }

        protected void ProcessForDeletion(UIElement control)
        {
            //Overlay Overlay = Owner as Overlay;

            //Window window = control as Window;

            //if (Overlay != null && window != null)
            //    Overlay.WindowManager.Remove(window);

            control.CanRaiseEvents = false;
            control.IsBeingRemoved = true;

            IContainer containerControl = control as IContainer;

            if (containerControl != null)
                foreach (UIElement childControl in containerControl.PrivateControlCollection.AllControls)
                {
                    childControl.CanRaiseEvents = false;
                    childControl.IsBeingRemoved = true;
                }
        }

        protected void ProcessForInsertion(UIElement control)
        {
            Contract.Requires<ArgumentNullException>(control != null);
            Contract.Requires<InvalidOperationException>(!Contains(control), "Control is already in collection.");

            //Window window = control as Window;
            //if (window != null)
            //    if (Owner is Overlay)
            //        windowLayer = OdysseyUI.CurrentOverlay.WindowManager.RegisterWindow(window);
            //    else
            //        throw new ArgumentException("Windows can only be added to the Overlay.");
            control.Parent = Owner;
            control.Depth = Depth.AsChildOf(Owner.Depth);
            control.DesignMode = Owner.DesignMode;
        }

        protected override void RemoveItem(int index)
        {
            ProcessForDeletion(this[index]);
            base.RemoveItem(index);
        }
    }
}