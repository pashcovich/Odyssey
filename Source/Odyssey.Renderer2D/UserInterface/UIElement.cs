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
using System.Collections.Generic;
using System.Diagnostics;
using Odyssey.Animations;
using Odyssey.Core;
using Odyssey.Serialization;
using Odyssey.UserInterface.Behaviors;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Data;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.UserInterface
{
    /// <summary>
    ///     The <b>UIElement</b> class is the root class of all controls in the library. It provides
    ///     inheritors with a comprehensive range of properties and methods common to all controls.
    /// </summary>
    [DebuggerDisplay("[{GetType().Name}]: {Name}")]
    public abstract partial class UIElement : Component, IAnimator, IComparable<UIElement>, ISerializableResource
    {
        private static readonly Dictionary<string, int> TypeCounter = new Dictionary<string, int>();

        #region Private fields

        private readonly AnimationController animator;
        private readonly BehaviorCollection behaviors;
        private readonly Dictionary<string, BindingExpression> bindings;
        private Vector3 absolutePosition;
        private RectangleF boundingRectangle;
        private bool canRaiseEvents = true;
        private object dataContext;
        private float depth;
        private bool designMode = true;
        private float height;
        private HorizontalAlignment horizontalAlignment;
        private bool isEnabled = true;
        private bool isFocusable = true;
        private bool isHighlighted;
        private bool isInside;
        private bool isSelected;
        private bool isVisible = true;
        private float maximumDepth;
        private float maximumHeight;
        private float maximumWidth;
        private float minimumDepth;
        private float minimumHeight;
        private float minimumWidth;
        private UIElement parent;
        private Vector3 position;
        private Vector3 renderSize;
        private Matrix3x2 transform;

        private VerticalAlignment verticalAlignment;
        private float width;

        #endregion Private fields

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UIElement" /> class.
        /// </summary>
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

            Children = new UIElementCollection(this);
            bindings = new Dictionary<string, BindingExpression>();
            behaviors = new BehaviorCollection();
            animator = new AnimationController(this);
            DependencyProperties = new PropertyContainer(this);

            width = float.NaN;
            height = float.NaN;
            depth = float.NaN;
            MinimumWidth = 4;
            MinimumHeight = 4;
            MinimumDepth = 0;
            MaximumWidth = float.PositiveInfinity;
            MaximumHeight = float.PositiveInfinity;
            MaximumDepth = float.PositiveInfinity;
            verticalAlignment = VerticalAlignment.Stretch;
            horizontalAlignment = HorizontalAlignment.Stretch;
        }

        #endregion Constructors

        public int CompareTo(UIElement other)
        {
            return Depth.CompareTo(other.Depth);
        }

        /// <summary>
        ///     Computes the intersection between the cursor location and this control. It is called
        ///     each time an event is fired on every control in the <see cref="Odyssey.UserInterface.Controls.Overlay" /> to
        ///     determine
        ///     if the UI needs to react.
        /// </summary>
        /// <param name="cursorLocation">The location of the mouse cursor</param>
        /// <returns>
        ///     <b>True</b> if the cursor is inside the control's boundaries. <b>False</b>,
        ///     otherwise.
        /// </returns>
        public virtual bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        public abstract void Render();

        public override string ToString()
        {
            return string.Format("{0}: '{1}' [{2}]", GetType().Name, Name, AbsolutePosition);
        }

        private void UpdateLayoutInternal()
        {
            boundingRectangle = new RectangleF(AbsolutePosition.X, AbsolutePosition.Y, RenderSize.X, RenderSize.Y);
            transform = Matrix3x2.Translation(AbsolutePosition.X, AbsolutePosition.Y);
        }
    }
}