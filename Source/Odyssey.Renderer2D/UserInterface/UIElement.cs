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

using System.Xml;
using System.Xml.Serialization;
using Odyssey.Serialization;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Serialization;
using Odyssey.UserInterface.Style;
using Odyssey.UserInterface.Xml;
using Odyssey.Utilities.Text;
using SharpDX;
using System;
using System.Collections.Generic;

#endregion Using Directives

namespace Odyssey.UserInterface
{
    /// <summary>
    /// The <b>UIElement</b> class is the root class of all controls in the library. It provides
    /// inheritors with a comprehensive range of properties and methods common to all controls.
    /// </summary>
    public abstract partial class UIElement : Component, IUIElement, IComparable<UIElement>, IStyleSerializable
    {
        protected static readonly Dictionary<string, int> TypeCounter = new Dictionary<string, int>();

        #region Private fields

        private readonly Dictionary<string, BindingExpression> bindings;
        private RectangleF boundingRectangle;
        private bool canRaiseEvents = true;
        private bool designMode = true;

        private bool isEnabled = true;
        private bool isFocusable = true;
        private bool isHighlighted;
        private bool isInside;
        private bool isSelected;
        private bool isVisible = true;
        private UIElement parent;

        private Vector2 position;

        #endregion Private fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UIElement" /> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks>
        /// </remarks>
        protected UIElement()
        {
            string type = GetType().Name;
            if (!TypeCounter.ContainsKey(type))
                TypeCounter.Add(type, 1);
            else
                ++TypeCounter[type];

            Name = string.Format("{0}{1}", type, TypeCounter[type]);
            bindings = new Dictionary<string, BindingExpression>();
        }

        #endregion Constructors

        public int CompareTo(UIElement other)
        {
            return Depth.CompareTo(other.Depth);
        }

        /// <summary>
        /// Computes the intersection between the cursor location and this control. It is called
        /// each time an event is fired on every control in the <see cref="Odyssey.UserInterface.Controls.Overlay"/> to determine
        /// if the UI needs to react.
        /// </summary>
        /// <param name="cursorLocation">The location of the mouse cursor</param>
        /// <returns><b>True</b> if the cursor is inside the control's boundaries. <b>False</b>,
        /// otherwise.</returns> <seealso cref="Intersection"/>
        public abstract bool Contains(Vector2 cursorLocation);

        public abstract void Render();

        public override string ToString()
        {
            return string.Format("{0}: '{1}' [{2}] D:{3}", GetType().Name, Name, AbsolutePosition, Depth);
        }

        /// <summary>
        /// Computes the absolute position of the control, depending on the inherited position of
        /// the parent. This method is called when its position or the parent changes.
        /// </summary>
        protected internal virtual void Layout()
        {
            if (parent != null)
            {
                Vector2 oldAbsolutePosition = AbsolutePosition;
                Vector2 newAbsolutePosition = new Vector2(parent.AbsolutePosition.X + position.X,
                    parent.AbsolutePosition.Y + position.Y);

                if (!newAbsolutePosition.Equals(oldAbsolutePosition))
                {
                    AbsolutePosition = newAbsolutePosition;
                }

                Measure();
                OnLayoutUpdated(EventArgs.Empty);
            }
        }

        protected virtual void Measure()
        {
            if (Width == 0 && Parent != null)
            {
                Width = Parent.Width - Margin.Horizontal;
                Control parentControl = (Parent as Control);
                if (parentControl != null)
                    Width -= parentControl.Padding.Horizontal;
            }
            if (Height == 0 && Parent != null)
            {
                Height = Parent.Height - Margin.Vertical;
                Control parentControl = (Parent as Control);
                if (parentControl != null)
                    Height -= parentControl.Padding.Vertical;
            }

            boundingRectangle = new RectangleF(AbsolutePosition.X, AbsolutePosition.Y, Width, Height);
            transform = Matrix3x2.Translation(AbsolutePosition.X, AbsolutePosition.Y);
        }


    }
}