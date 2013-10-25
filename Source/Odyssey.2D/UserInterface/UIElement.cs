using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;

namespace Odyssey.UserInterface
{
    /// <summary>
    /// The <b>UIElement</b> class is the root class of all controls in the library. It provides
    /// inheritors with a comprehensive range of properties and methods common to all controls.
    /// </summary>
    public abstract partial class UIElement : Component, IUIElement, IComparable<UIElement>
    {
        #region Private fields

        private RectangleF boundingRectangle;
        private bool canRaiseEvents = true;
        private bool designMode = true;
        private bool disposed;

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
        protected UIElement(string id)
        {
            Id = id;
        }

        #endregion Constructors

        public int CompareTo(UIElement other)
        {
            return Depth.CompareTo(other.Depth);
        }

        /// <summary>
        /// Computes the intersection between the cursor location and this control. It is called
        /// each time an event is fired on every control in the <see cref="Overlay"/> to determine
        /// if the UI needs to react.
        /// </summary>
        /// <param name="cursorLocation">The location of the mouse cursor</param>
        /// <returns><b>True</b> if the cursor is inside the control's boundaries. <b>False</b>,
        /// otherwise.</returns> <seealso cref="Intersection"/>
        public abstract bool Contains(Vector2 cursorLocation);

        public override string ToString()
        {
            return string.Format("{0}: '{1}' [{2}] D:{3}", GetType().Name, Id, AbsolutePosition, Depth);
        }

        /// <summary>
        /// Computes the absolute position of the control, depending on the inherited position of
        /// the parent. This method is called when its position or the parent changes.
        /// </summary>
        protected internal virtual void Arrange()
        {
            if (parent != null)
            {
                Vector2 oldAbsolutePosition = AbsolutePosition;
                Vector2 newAbsolutePosition =
                    new Vector2(parent.AbsolutePosition.X + position.X, parent.AbsolutePosition.Y + position.Y);

                if (!IsSubComponent)
                    newAbsolutePosition += parent.TopLeftPosition;

                if (!newAbsolutePosition.Equals(oldAbsolutePosition))
                {
                    AbsolutePosition = newAbsolutePosition;
                    OnPositionChanged(EventArgs.Empty);
                }

                BoundingRectangle = new RectangleF(AbsolutePosition.X, AbsolutePosition.Y, Width, Height);
            }
        }
    }
}