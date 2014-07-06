#region Using Directives

using Odyssey.Interaction;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using System;

#endregion Using Directives

namespace Odyssey.UserInterface
{
    public abstract partial class UIElement
    {
        #region PointerEvents

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
        /// Occurs when mouse pointer is over the control and a mouse button is pressed..
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerPressed;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerReleased;

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerWheelChanged;

        /// <summary>
        /// Occurs when the control is clicked by the mouse.
        /// </summary>
        public event EventHandler<PointerEventArgs> Tap;

        /// <summary>
        /// Raises the <see cref="PointerExited"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected internal virtual void OnPointerExited(PointerEventArgs e)
        {
            isInside = false;
            if (CanRaiseEvents)
            {
                if (e.CurrentPoint.IsLeftButtonPressed)
                {
                    //if (UserInterfaceManager.CurrentOverlay.PressedControl == this)
                    //{
                    //    IsHighlighted = IsPressed = false;
                    //}
                    //else
                    //    return;
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

            if (Overlay.State.Entered.IsInside)
                Overlay.State.Entered.OnPointerExited(e);

            Overlay.State.Entered = this;

            if (CanRaiseEvents)
            {
                if (e.CurrentPoint.IsLeftButtonPressed)
                {
                    //if (UserInterfaceManager.CurrentOverlay.PressedControl == this)
                    //{
                    //    IsHighlighted = IsPressed = true;
                    //}
                    //else
                    //    return;
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
        /// Raises the <see cref="PointerPressed"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPointerPressed(PointerEventArgs e)
        {
            //if (UserInterfaceManager.CurrentOverlay.FocusedControl != this)
            //    UserInterfaceManager.CurrentOverlay.FocusedControl.OnLostFocus(e);

            OnUpdate(e);

            if (PointerPressed != null)
                PointerPressed(this, e);
        }

        /// <summary>
        /// Raises the <see cref="PointerReleased"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PointerEventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPointerReleased(PointerEventArgs e)
        {
            //UserInterfaceManager.CurrentOverlay.PressedControl = null;

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
        /// Occurs when the <see cref="DesignMode"/> property value changes.
        /// </summary>
        public event EventHandler<ControlEventArgs> DesignModeChanged;

        /// <summary>
        /// Occurs when the control receives focus..
        /// </summary>
        public event EventHandler<EventArgs> GotFocus;

        /// <summary>
        /// Occurs when the <see cref="IsHighlighted"/> property value changes.
        /// </summary>
        public event EventHandler<EventArgs> HighlightedChanged;

        public event EventHandler<ControlEventArgs> Initialized;

        public event EventHandler<ControlEventArgs> Initializing;

        /// <summary>
        /// Occurs when the control's layout changes.
        /// </summary>
        public event EventHandler<EventArgs> LayoutUpdated;

        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        public event EventHandler<EventArgs> LostFocus;

        /// <summary>
        /// Occurs when the control is moved.
        /// </summary>
        public event EventHandler<EventArgs> Move;

        /// <summary>
        /// Occurs when the <see cref="Parent"/> property value changes.
        /// </summary>
        public event EventHandler<EventArgs> ParentChanged;

        /// <summary>
        /// Occurs when the control loses mouse capture.
        /// </summary>
        public event EventHandler<PointerEventArgs> PointerCaptureChanged;

        /// <summary>
        /// Occurs when the <see cref="Position"/> property value changes.
        /// </summary>
        public event EventHandler<EventArgs> PositionChanged;

        /// <summary>
        /// Occurs when <see cref="Size"/> property value changes.
        /// </summary>
        public event EventHandler<EventArgs> SizeChanged;

        public event EventHandler<EventArgs> Update;

        /// <summary>
        /// Occurs when the <see cref="IsVisible"/> property value changes.
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Occurs when the <see cref="IsSelected"/> property value changes.
        /// </summary>
        protected event EventHandler<EventArgs> SelectedChanged;

        #endregion Events declaration

        /// <summary>
        /// Raises the <see cref="PointerCaptureChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected internal virtual void OnPointerCaptureChanged(PointerEventArgs e)
        {
            //UserInterfaceManager.CurrentOverlay.CaptureControl = null;
            IsPressed = false;

            RaiseEvent(PointerCaptureChanged, this, e);
        }

        /// <summary>
        /// Raises the <see cref="DesignModeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnDesignModeChanged(ControlEventArgs e)
        {
            if (DesignModeChanged != null)
                DesignModeChanged(this, e);
        }

        /// <summary>
        /// Raises the <see cref="GotFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnGotFocus(EventArgs e)
        {
            //if (this != UserInterfaceManager.CurrentOverlay.FocusedControl)
            //{
            //    UserInterfaceManager.CurrentOverlay.FocusedControl.OnLostFocus(e);

            //    UserInterfaceManager.CurrentOverlay.FocusedControl = this;
            //    IsFocused = true;

            //    OnUpdate(e);

            //    if (GotFocus != null)
            //        GotFocus(this, e);
            //}
            //else return;

            RaiseEvent(GotFocus, this, e);
            RaiseEvent(Update, this, e);
        }

        /// <summary>
        /// Raises the <see cref="HighlightedChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnHighlightedChanged(EventArgs e)
        {
            RaiseEvent(HighlightedChanged, this, e);
            RaiseEvent(Update, this, e);
        }

        protected virtual void OnInitialized(ControlEventArgs e)
        {
            RaiseEvent(Initialized, this, e);
        }

        protected virtual void OnInitializing(ControlEventArgs e)
        {
            RaiseEvent(Initializing, this, e);
        }

        protected virtual void OnLayoutUpdated(EventArgs e)
        {
            RaiseEvent(LayoutUpdated, this, e);
        }

        /// <summary>
        /// Raises the <see cref="LostFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnLostFocus(EventArgs e)
        {
            IsFocused = IsPressed = false;
            //UserInterfaceManager.CurrentOverlay.FocusedControl = UserInterfaceManager.CurrentOverlay;

            RaiseEvent(LostFocus, this, e);
            RaiseEvent(Update, this, e);
        }

        /// <summary>
        /// Raises the <see cref="Move"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnMove(EventArgs e)
        {
            Layout();
            RaiseEvent(Move, this, e);
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnParentChanged(EventArgs e)
        {
            RaiseEvent(ParentChanged, this, e);
            if (DesignMode) return;
            RaiseEvent(Update, this, e);
        }

        /// <summary>
        /// Raises the <see cref="PositionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnPositionChanged(EventArgs e)
        {
            RaiseEvent(PositionChanged, this, e);
            RaiseEvent(Update, this, e);
        }

        /// <summary>
        /// Raises the <see cref="SelectedChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnSelectedChanged(EventArgs e)
        {
            RaiseEvent(SelectedChanged, this, e);
            RaiseEvent(Update, this, e);
        }

        /// <summary>
        /// Raises the <see cref="SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnSizeChanged(EventArgs e)
        {
            Layout();
            RaiseEvent(SizeChanged, this, e);
            RaiseEvent(Update, this, e);
        }

        protected virtual void OnUpdate(EventArgs e)
        {
            RaiseEvent(Update, this, e);
        }

        /// <summary>
        /// Raises the <see cref="VisibleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
            RaiseEvent(VisibleChanged, this, e);
            if (!DesignMode)
                RaiseEvent(Update, this, e);
        }

        protected void RaiseEvent<T>(EventHandler<T> handler, object sender, T args)
            where T : EventArgs
        {
            if (handler != null)
                handler(sender, args);
        }

        #endregion Control Events
    }
}