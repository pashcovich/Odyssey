using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Rendering2D;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Odyssey.UserInterface.Controls
{
    public abstract class ContainerControl : Control, IContainer
    {
        #region Properties

        //public override bool CanRaiseEvents
        //{
        //    get { return base.CanRaiseEvents; }
        //    set
        //    {
        //        if (CanRaiseEvents != value)
        //        {
        //            base.CanRaiseEvents = value;
        //            foreach (BaseControl ctl in controls.AllControls)
        //                ctl.CanRaiseEvents = false;
        //        }
        //    }
        //}

        protected ContainerControl(string controlDescriptionClass)
            : base(controlDescriptionClass)
        {
            PrivateControlCollection = new ControlCollection(this);
            IsFocusable = false;
        }

        /// <summary>
        /// Returns the publicly available collection of child controls.
        /// </summary>
        public virtual ControlCollection Controls
        {
            get { return PublicControlCollection; }
        }
        ControlCollection IContainer.PrivateControlCollection
        {
            get { return PrivateControlCollection; }
        }
        ControlCollection IContainer.PublicControlCollection
        {
            get { return PublicControlCollection; }
        }
        protected ControlCollection PrivateControlCollection { get; private set; }
        protected virtual ControlCollection PublicControlCollection
        {
            get { return PrivateControlCollection; }
        }

        #endregion Properties

        #region IContainer Members
        /// <summary>
        /// Occurs when a new control is added to the <see cref="ControlCollection"/>.
        /// </summary>
        public event EventHandler<ControlEventArgs> ControlAdded;

        /// <summary>
        /// Raises the <see cref="ControlAdded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Odyssey.UserInterface.ControlEventArgs"/> instance
        /// containing the event data.</param>
        protected internal virtual void OnControlAdded(ControlEventArgs e)
        {
            if (ControlAdded != null)
                ControlAdded(this, e);
        }



        #endregion IContainer Members

        protected internal override void Arrange()
        {
            base.Arrange();
            foreach (UIElement ctl in PublicControlCollection)
                ctl.Arrange();
            foreach (UIElement ctl in PrivateControlCollection)
                ctl.Arrange();

        }

        public override void Initialize(IDirectXProvider directX)
        {
            foreach (IRenderable control in PublicControlCollection)
                control.Initialize(directX);
        }

        public override void Render(IDirectXTarget target)
        {
            foreach (IControl control in PublicControlCollection)
                if (control.IsVisible)
                    control.Render(target);
        }

        public void Add(UIElement control)
        {
            PublicControlCollection.Add(ToDispose(control));
            OnControlAdded(new ControlEventArgs(control));
        }

        public virtual void AddRange(IEnumerable<UIElement> controls)
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
            Contract.Requires<ArgumentNullException>(control!= null, "control is null");

            return PublicControlCollection.Contains(control);
        }

        public UIElement Find(string id)
        {
            foreach (UIElement ctl in TreeTraversal.PreOrderControlVisit(this))
            {
                if (ctl.Id == id)
                    return ctl;
                else
                    continue;
            }
            return null;
        }

        public UIElement Find(Vector2 cursorLocation)
        {
            return TreeTraversal.PostOrderControlInteractionVisit(this)
                .Reverse()
                .FirstOrDefault(control => control.Contains(cursorLocation));
        }

        public void Insert(int index, UIElement control)
        {
            PublicControlCollection.Insert(index, control);
        }

        public void Remove(UIElement control)
        {
            PrivateControlCollection.Remove(control);
        }

        #region Debug

        internal static void Debug(IEnumerable<UIElement> iterator)
        {
            System.Diagnostics.Debug.WriteLine("---------");
            foreach (UIElement ctl in iterator)
            {
                System.Diagnostics.Debug.WriteLine(ctl.Id);
            }
            System.Diagnostics.Debug.WriteLine("---------");
        }

        #endregion Debug
    }
}