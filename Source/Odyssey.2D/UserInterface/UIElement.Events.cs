using Odyssey.Devices;
using Odyssey.UserInterface.Controls;
using System;

namespace Odyssey.UserInterface
{
    public abstract partial class UIElement
    {
        #region PointerEvents

        /// <summary>
        /// Occurs when the control is clicked by the mouse.
        /// </summary>
        public event EventHandler<PointerEventArgs> Tap;

        /// <summary>
        /// Occurs when mouse pointer is over the control and a mouse button is pressed..
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerPressed;

        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerEntered;

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerExited;

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerMoved;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerReleased;

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerWheelChanged;

        /// <summary>
        /// Raises the <see cref="Tap"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnTap(PointerEventArgs e)
        {
            OnUpdate(e);

            if (Tap != null)
                Tap(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PointerPressed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPointerPressed(PointerEventArgs e)
        {
            if (OdysseyUI.CurrentOverlay.FocusedControl != this)
                OdysseyUI.CurrentOverlay.FocusedControl.OnLostFocus(e);

            OnUpdate(e);

            if (PointerPressed != null)
                PointerPressed(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PointerEntered"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPointerEnter(PointerEventArgs e)
        {
            //DebugManager.LogToScreen(
            //    DateTime.Now.Millisecond + " Entering " +
            //    id + " H: " + isHighlighted);

            isInside = true;
            if (OdysseyUI.CurrentOverlay.EnteredControl.IsInside)
                OdysseyUI.CurrentOverlay.EnteredControl.OnPointerExited(e);

            OdysseyUI.CurrentOverlay.EnteredControl = this;

            if (CanRaiseEvents)
            {
                if (e.CurrentPoint.Properties.IsLeftButtonPressed)
                {
                    if (OdysseyUI.CurrentOverlay.PressedControl == this)
                    {
                        IsHighlighted = IsPressed = true;
                    }
                    else
                        return;
                }
                else
                {
                    IsHighlighted = true;
                }
            }

            if (PointerEntered != null)
                PointerEntered(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PointerExited"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPointerExited(PointerEventArgs e)
        {
            isInside = false;
            if (CanRaiseEvents)
            {
                if (e.CurrentPoint.Properties.IsLeftButtonPressed)
                {
                    if (OdysseyUI.CurrentOverlay.PressedControl == this)
                    {
                        IsHighlighted = IsPressed = false;
                    }
                    else
                        return;
                }
                else
                {
                    IsHighlighted = false;
                }
            }

            if (PointerExited != null)
                PointerExited(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PointerMoved"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPointerMove(PointerEventArgs e)
        {
            if (PointerMoved != null)
                PointerMoved(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PointerReleased"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPointerReleased(PointerEventArgs e)
        {
            OdysseyUI.CurrentOverlay.PressedControl = null;

            OnUpdate(e);

            if (PointerReleased != null)
                PointerReleased(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PointerWheelChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPointerWheelChanged(PointerEventArgs e)
        {
            if (PointerWheelChanged != null)
                PointerWheelChanged(this, e);
        }

        #endregion PointerEvents

        #region Keyboard Events

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyDown;

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        //public event KeyPressEventHandler KeyPress;
        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUp;

        /// <summary>
        /// Raises the <see cref="KeyDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            if (KeyDown != null)
                KeyDown(this, e);
        }

        /// <summary>
        /// Raises the <see cref="KeyPress"/> event.
        /// </summary>
        /// <param name="e">The <see cref=".KeyPressEventArgs"/> instance containing the event data.</param>
        //protected virtual void OnKeyPress(KeyPressEventArgs e)
        //{
        //    KeyPressEventHandler handler = (KeyPressEventHandler) Events[EventKeyPress];
        //    if (handler != null)
        //        handler(this, e);
        //}

        /// <summary>
        /// Raises the <see cref="KeyUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            if (KeyUp != null)
                KeyUp(this, e);
        }

        #endregion Keyboard Events

        #region Control Events

        #region Events declaration

       

        /// <summary>
        /// Occurs when the <see cref="Description"/> property value changes.
        /// </summary>
        public event EventHandler ControlStyleChanged;

        /// <summary>
        /// Occurs when the <see cref="DesignMode"/> property value changes.
        /// </summary>
        public event EventHandler<ControlEventArgs> DesignModeChanged;

        public event EventHandler<ControlEventArgs> Initialized;

        /// <summary>
        /// Occurs when the control receives focus..
        /// </summary>
        public event EventHandler GotFocus;

        /// <summary>
        /// Occurs when the <see cref="IsHighlighted"/> property value changes.
        /// </summary>
        public event EventHandler HighlightedChanged;

        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        public event EventHandler LostFocus;

        /// <summary>
        /// Occurs when the control loses mouse capture.
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerCaptureChanged;

        /// <summary>
        /// Occurs when the control is moved.
        /// </summary>
        public event EventHandler Move;

        /// <summary>
        /// Occurs when the <see cref="Parent"/> property value changes.
        /// </summary>
        public event EventHandler ParentChanged;

        /// <summary>
        /// Occurs when the <see cref="Position"/> property value changes.
        /// </summary>
        public event EventHandler PositionChanged;

        /// <summary>
        /// Occurs when <see cref="Size"/> property value changes.
        /// </summary>
        public event EventHandler SizeChanged;

        /// <summary>
        /// Occurs when the <see cref="TextStyle"/> property value changes.
        /// </summary>
        public event EventHandler TextStyleChanged;

        public event EventHandler Update;

        /// <summary>
        /// Occurs when the <see cref="IsVisible"/> property value changes.
        /// </summary>
        public event EventHandler VisibleChanged;

        /// <summary>
        /// Occurs when the <see cref="IsSelected"/> property value changes.
        /// </summary>
        protected event EventHandler SelectedChanged;

        #endregion Events declaration


        /// <summary>
        /// Raises the <see cref="ControlStyleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnControlStyleChanged(EventArgs e)
        {
            if (!DesignMode)
                OnUpdate(e);

            if (ControlStyleChanged != null)
                ControlStyleChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DesignModeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnDesignModeChanged(ControlEventArgs e)
        {
            if (DesignModeChanged != null)
                DesignModeChanged(this, e);
        }

        protected virtual void OnInitialized(ControlEventArgs e)
        {
            if (Initialized != null)
                Initialized(this, e);
        }

        /// <summary>
        /// Raises the <see cref="GotFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnGotFocus(EventArgs e)
        {
            if (this != OdysseyUI.CurrentOverlay.FocusedControl)
            {
                OdysseyUI.CurrentOverlay.FocusedControl.OnLostFocus(e);

                OdysseyUI.CurrentOverlay.FocusedControl = this;
                IsFocused = true;

                OnUpdate(e);

                if (GotFocus != null)
                    GotFocus(this, e);
            }
            else return;
        }

        /// <summary>
        /// Raises the <see cref="HighlightedChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnHighlightedChanged(EventArgs e)
        {
            OnUpdate(e);

            if (HighlightedChanged != null)
                HighlightedChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="LostFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnLostFocus(EventArgs e)
        {
            IsFocused = IsPressed = false;
            OdysseyUI.CurrentOverlay.FocusedControl = OdysseyUI.CurrentOverlay;

            OnUpdate(e);

            if (LostFocus != null)
                LostFocus(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PointerCaptureChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected internal virtual void OnPointerCaptureChanged(PointerEventArgs e)
        {
            OdysseyUI.CurrentOverlay.CaptureControl = null;
            IsPressed = false;

            if (PointerCaptureChanged != null)
                PointerCaptureChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Move"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnMove(EventArgs e)
        {
            if (Move != null)
                Move(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnParentChanged(EventArgs e)
        {
            Depth = Style.Depth.AsChildOf(parent.Depth);

            IContainer iContainer = this as IContainer;
            if (iContainer == null) return;
            foreach (UIElement ctl in TreeTraversal.PreOrderControlVisit(iContainer))
            //foreach (BaseControl ctl in iContainer.PrivateControlCollection.AllControls)
            {
                ctl.Depth = Style.Depth.AsChildOf(ctl.Parent.Depth);
            }

            if (DesignMode) return;

            Arrange();
            OnUpdate(e);

            if (ParentChanged != null)
                ParentChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PositionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPositionChanged(EventArgs e)
        {
            if (parent != null)
                Arrange();

            OnUpdate(e);

            if (PositionChanged != null)
                PositionChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="SelectedChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnSelectedChanged(EventArgs e)
        {
            OnUpdate(e);

            if (SelectedChanged != null)
                SelectedChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnSizeChanged(EventArgs e)
        {
            if (parent != null)
                Arrange();

            OnUpdate(e);

            if (SizeChanged != null)
                SizeChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="TextStyleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnTextDescriptionChanged(EventArgs e)
        {
            if (TextStyleChanged != null)
                TextStyleChanged(this, e);
        }

        protected virtual void OnUpdate(EventArgs e)
        {

            if (Update != null)
                Update(this, e);
        }

        /// <summary>
        /// Raises the <see cref="VisibleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Engine.EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
            if (!DesignMode)
                OnUpdate(e);

            if (VisibleChanged != null)
                VisibleChanged(this, e);
        }

        #endregion Control Events
    }
}