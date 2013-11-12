using Odyssey.Devices;
using Odyssey.Engine;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using MouseEventArgs = Odyssey.Devices.PointerEventArgs;

namespace Odyssey.UserInterface
{
    public abstract partial class UIElement
    {

        #region Public methods

        public void BringToFront()
        {
            Depth = new Depth(Depth.WindowLayer, Depth.ComponentLayer, Depth.Foreground);
        }

        /// <summary>
        /// Programmatically focuses this <see cref="UIElement"/> object, <b>if</b> it is focusable.
        /// </summary>
        public void Focus()
        {
            if (isFocusable)
                OnGotFocus(EventArgs.Empty);
        }

        public void SendToBack()
        {
            Depth = new Depth(Depth.WindowLayer, Depth.ComponentLayer, Depth.Background);
        }

        ///// <summary>
        ///// Returns the window that this control belongs to, if any.
        ///// </summary>
        ///// <returns>The <see cref="Window"/> reference the control belongs to; <c>null</c> if the control doesn't
        ///// belong to any window.</returns>
        //public Window FindWindow()
        //{
        //    if (depth.WindowLayer == 0)
        //        return null;
        //    else
        //        return OdysseyUI.CurrentOverlay.WindowManager[depth.WindowLayer - 1];
        //}

        public abstract void Initialize(IDirectXProvider directX);

        #endregion Public methods

        #region Event Processing methods

        internal virtual void ProcessKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        internal virtual void ProcessKeyUp(KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        internal virtual bool ProcessPointerMovement(PointerEventArgs e)
        {
            Vector2 location = e.CurrentPoint.Position;
            if (!HasCaptured && !Contains(location))
                return false;

            if (canRaiseEvents)
            {
                if (!isInside)
                    OnPointerEnter(e);

                OnPointerMove(e);
                return true;
            }
            return false;
        }

        internal virtual bool ProcessPointerPressed(PointerEventArgs e)
        {
            Vector2 location = e.CurrentPoint.Position;
            if (!canRaiseEvents || !Contains(location))
                return false;

            if (!IsPressed && isEnabled)
            {
                if (e.CurrentPoint.Properties.IsLeftButtonPressed)
                    IsPressed = true;
                OnPointerPressed(e);

                if (isFocusable && !IsFocused)
                    OnGotFocus(EventArgs.Empty);
            }

            return true;
        }

        internal virtual bool ProcessPointerRelease(PointerEventArgs e)
        {
            Vector2 location = e.CurrentPoint.Position;
            if (canRaiseEvents && (HasCaptured || Contains(location)))
            {
                if (IsPressed && e.CurrentPoint.Properties.IsLeftButtonPressed)
                {
                    OnTap(e);
                    IsPressed = false;
                }
                OnPointerReleased(e);
                return true;
            }

            if (IsPressed)
                IsPressed = false;
            return false;
        }
        #endregion


        #region Static Methods

        /// <summary>
        /// Computes the eight points bounding this control.
        /// <remarks>
        /// Index 0 is northwest corner.<br>Index 7 is west /br>
        /// </remarks>
        /// </summary>
        /// <param name="control">The control whose bounds to compute.</param>
        /// <returns>The array of points, stored in clockwise order.</returns>
        public static Vector2[] ComputeBounds(UIElement control)
        {
            Vector2 cornerNE = control.AbsolutePosition;
            float width = control.Width;
            float height = control.Height;

            return new[]{
                           cornerNE,
                           new Vector2(cornerNE.X + width/2f, cornerNE.Y),
                           new Vector2(cornerNE.X + width, cornerNE.Y),
                           new Vector2(cornerNE.X + width, cornerNE.Y + height/2f),
                           new Vector2(cornerNE.X + width, cornerNE.Y + height),
                           new Vector2(cornerNE.X + width/2f, cornerNE.Y + height),
                           new Vector2(cornerNE.X, cornerNE.Y + height),
                           new Vector2(cornerNE.X, cornerNE.Y + height/2f)
                       };
        }

        #endregion Static Methods

        public static explicit operator RectangleF (UIElement uiElement)
        {
            float x = uiElement.AbsolutePosition.X;
            float y = uiElement.AbsolutePosition.Y;
            float width = uiElement.Width;
            float height = uiElement.Height;


            return new RectangleF(x, y, width, height);
        }
    }
}