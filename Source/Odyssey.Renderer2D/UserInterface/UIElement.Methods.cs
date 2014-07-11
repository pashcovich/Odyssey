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

using Odyssey.Interaction;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Diagnostics.Contracts;
using MouseEventArgs = Odyssey.Interaction.PointerEventArgs;

#endregion Using Directives

namespace Odyssey.UserInterface
{
    public abstract partial class UIElement
    {
        #region Public methods

        /// <summary>
        /// Programmatically focuses this <see cref="UIElement"/> object, <b>if</b> it is focusable.
        /// </summary>
        public void Focus()
        {
            if (isFocusable)
                OnGotFocus(EventArgs.Empty);
        }

        public void Initialize()
        {
            ControlEventArgs args = new ControlEventArgs(this);
            OnInitializing(args);
            foreach (var kvp in bindings)
            {
                var bindingExpression = kvp.Value;
                bindingExpression.SourceBinding.Source = DataContext;
                bindingExpression.Initialize();
            }

            OnInitialized(args);
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
        //        return UserInterfaceManager.CurrentOverlay.WindowManager[depth.WindowLayer - 1];
        //}
        public virtual void Unload()
        {
            Dispose();
        }

        public void BringToFront()
        {
            Depth = new Depth(Depth.WindowLayer, Depth.ComponentLayer, Depth.Foreground);
        }

        public void SendToBack()
        {
            Depth = new Depth(Depth.WindowLayer, Depth.ComponentLayer, Depth.Background);
        }

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

        internal virtual bool ProcessPointerMovement(MouseEventArgs e)
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

        internal virtual bool ProcessPointerPressed(MouseEventArgs e)
        {
            Vector2 location = e.CurrentPoint.Position;
            if (!canRaiseEvents || !Contains(location))
                return false;

            if (!IsPressed && isEnabled)
            {
                if (e.CurrentPoint.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
                    IsPressed = true;
                OnPointerPressed(e);

                if (isFocusable && !IsFocused)
                    OnGotFocus(EventArgs.Empty);
            }

            return true;
        }

        internal virtual bool ProcessPointerRelease(MouseEventArgs e)
        {
            Vector2 location = e.CurrentPoint.Position;
            if (canRaiseEvents && (HasCaptured || Contains(location)))
            {
                if (IsPressed && e.CurrentPoint.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
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

        #endregion Event Processing methods

        public static explicit operator RectangleF(UIElement uiElement)
        {
            float x = uiElement.AbsolutePosition.X;
            float y = uiElement.AbsolutePosition.Y;
            float width = uiElement.Width;
            float height = uiElement.Height;

            return new RectangleF(x, y, width, height);
        }

        public void SetBinding(Binding binding, string targetProperty)
        {
            Contract.Requires<ArgumentNullException>(binding != null, "binding");

            var bindingExpression = new BindingExpression(binding, this, targetProperty);

            bindings.Add(targetProperty, bindingExpression);
        }

        /// <summary>
        /// Creates a shallow copy of this object and its children.
        /// </summary>
        /// <returns>A new copy of this element.</returns>
        internal virtual UIElement Copy()
        {
            UIElement newElement = (UIElement) Activator.CreateInstance(GetType());

            newElement.Name = Name;
            newElement.Width = Width;
            newElement.Height = Height;
            newElement.Margin = Margin;

            return newElement;
        }
    }
}