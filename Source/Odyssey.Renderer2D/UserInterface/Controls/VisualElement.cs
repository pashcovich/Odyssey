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

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using Odyssey.Content;
using Odyssey.Graphics.Drawing;
using Odyssey.Reflection;
using Odyssey.Text.Logging;
using Odyssey.UserInterface.Data;
using Odyssey.UserInterface.Style;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.UserInterface.Controls
{
    public abstract class VisualElement : UIElement, IResourceProvider
    {
        private string controlStyleClass;
        private VisualStyle style;
        private TextStyle textStyle;
        private string textStyleClass;
        private VisualState visualState;

        protected VisualElement(string controlStyleClass, string textStyleClass = TextStyle.Default)
        {
            this.controlStyleClass = controlStyleClass;
            this.textStyleClass = textStyleClass;
        }

        public string StyleClass
        {
            get { return controlStyleClass; }
            set
            {
                if (string.Equals(controlStyleClass, value))
                    return;
                controlStyleClass = value;
                if (!DesignMode)
                    ApplyVisualStyle();
            }
        }

        private bool IsVisual
        {
            get { return VisualState != null; }
        }

        public DataTemplate Template { get; set; }

        /// <summary>
        ///     Gets or sets the <see cref="TextStyle" /> to use for this control.
        /// </summary>
        /// <value>
        ///     The <see cref="TextStyle" /> object that contains information on how to
        ///     format the text of this control.
        /// </value>
        public TextStyle TextStyle
        {
            get { return textStyle; }
            private set
            {
                if (textStyle == value) return;
                textStyle = value;

                OnTextStyleChanged(EventArgs.Empty);
            }
        }

        public string TextStyleClass
        {
            get { return textStyleClass; }
            set
            {
                if (string.Equals(textStyleClass, value))
                    return;

                textStyleClass = value;

                if (DesignMode)
                    return;

                ApplyTextStyle();
                InvalidateMeasure();
                OnTextStyleChanged(EventArgs.Empty);
            }
        }

        protected ControlStatus ActiveStatus { get; set; }

        protected VisualState VisualState
        {
            get { return visualState; }
        }

        /// <summary>
        ///     Gets or sets the <see cref="Style" /> to use for this control.
        /// </summary>
        /// <value>
        ///     The <see cref="Style" /> object that contains information on how to style
        ///     the text appearance of this control.
        /// </value>
        public VisualStyle Style
        {
            get { return style; }
            private set
            {
                if (style == value) return;

                style = value;
                OnControlDefinitionChanged(EventArgs.Empty);
            }
        }

        public override void Render()
        {
            foreach (var child in Children)
                if (child.IsVisible)
                    child.Render();
        }

        /// <summary>
        ///     Occurs when the <see cref="VisualStyle" /> property value changes.
        /// </summary>
        public event EventHandler<EventArgs> ControlStyleChanged;

        /// <summary>
        ///     Occurs when the <see cref="UserInterface.Style.TextStyle" /> property value changes.
        /// </summary>
        public event EventHandler<EventArgs> TextStyleChanged;

        protected internal override UIElement Copy()
        {
            var newElement = (VisualElement) base.Copy();
            newElement.Width = Width;
            newElement.Height = Height;
            newElement.Style = Style;
            newElement.TextStyleClass = TextStyleClass;
            newElement.Template = Template;
            CopyEvents(typeof (VisualElement), this, newElement);
            return newElement;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            foreach (var child in Children)
                child.Arrange(child.IsInternal ? availableSizeWithoutMargins : availableSizeWithoutMargins - MarginInternal);
            return availableSizeWithoutMargins;
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            foreach (var child in Children)
                child.Measure(child.IsInternal ? availableSizeWithoutMargins : availableSizeWithoutMargins - MarginInternal);

            if (float.IsNaN(Width) || float.IsNaN(Height) || float.IsNaN(Depth))
            {
                return availableSizeWithoutMargins - MarginInternal;
            }
            return Size - MarginInternal;
        }

        private void ApplyVisualStyle()
        {
            if (!Overlay.Theme.ContainsResource(StyleClass))
            {
                LogEvent.UserInterface.Warning("Style '{0}' not found", StyleClass);
                return;
            }
            var visualStyle = Overlay.Theme.GetResource<VisualStyle>(StyleClass);

            if (float.IsNaN(Width) && visualStyle.Width > 0)
            {
                Width = visualStyle.Width;
            }

            if (float.IsNaN(Height) && visualStyle.Height > 0)
            {
                Height = visualStyle.Height;
            }

            RemoveAndDispose(ref visualState);
            visualState = ToDispose(visualStyle.CreateVisualState(this));
            visualState.Initialize();

            foreach (var s in visualState)
                SetParent(s, this);

            Style = visualStyle;

            if (Animator.HasAnimations)
                Animator.Initialize();

            InitializeTriggers();

        }

        private void InitializeTriggers()
        {
            if (!Triggers.HasTriggers)
                return;

            foreach (var trigger in Triggers)
            {
                trigger.Initialize(this);
            }

        }

        protected virtual void CreateDefaultTemplate()
        { }

        private void ApplyTemplate()
        {
            if (Template != null)
            {
                var control = ToDispose(Template.VisualTree.Copy());
                foreach (var element in TreeTraversal.PreOrderVisit(control))
                {
                    string elementName = element.Name;
                    foreach (var kvp in Template.Bindings)
                    {
                        var binding = kvp.Value;
                        if (string.Equals(kvp.Value.TargetElement, elementName))
                            element.SetBinding(binding, kvp.Key);
                    }
                }

                var cContent = this as ContentControl;
                if (cContent != null && control is VisualElement)
                    cContent.Content=control;
                else
                   SetParent(control, this);
            }
        }

        private void ApplyTextStyle()
        {
            if (string.Equals(TextStyleClass, Binding.TemplatedParent) || string.Equals(TextStyleClass, TextStyle.Default))
            {
                Control parent = FindAncestor<ItemsControl>() ?? (Control) FindAncestor<ContentControl>() ?? Overlay;
                TextStyleClass = parent.TextStyleClass;
            }
            
            if (!Overlay.Theme.ContainsResource(TextStyleClass))
            {
                LogEvent.UserInterface.Warning("TextStyle '{0}' not found", TextStyleClass);
                return;
            }

            TextStyle = Overlay.Theme.GetResource<TextStyle>(TextStyleClass);

        }

        /// <summary>
        ///     Raises the <see cref="ControlStyleChanged" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="EventArgs" /> instance containing the event
        ///     data.
        /// </param>
        protected virtual void OnControlDefinitionChanged(EventArgs e)
        {
            if (!DesignMode)
                RaiseEvent(ControlStyleChanged, this, e);
        }

        protected override void OnDesignModeChanged(EventArgs e)
        {
            base.OnDesignModeChanged(e);
            if (!IsVisual || ActiveStatus == ControlStatus.None)
                return;

            foreach (Shape element in VisualState)
                element.DesignMode = DesignMode;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (string.Equals(StyleClass, "Empty")) return;
            ApplyVisualStyle();
            ActiveStatus = IsEnabled ? ControlStatus.Enabled : ControlStatus.Disabled;
        }

        protected override void OnInitializing(EventArgs e)
        {
            base.OnInitializing(e);
            CreateDefaultTemplate();
            ApplyTemplate();
            ApplyTextStyle();
        }

        /// <summary>
        ///     Raises the <see cref="TextStyleChanged" /> event.
        /// </summary>
        /// <param name="e">
        ///     The <see cref="EventArgs" /> instance containing the event
        ///     data.
        /// </param>
        protected virtual void OnTextStyleChanged(EventArgs e)
        {
            if (!DesignMode)
                RaiseEvent(TextStyleChanged, this, e);
        }

        public Shape GetShape(string shapeName)
        {
            return VisualState[shapeName];
        }

        #region IResourceProvider

        protected virtual IEnumerable<IResource> Resources
        {
            get { return VisualState; }
        }

        TResource IResourceProvider.GetResource<TResource>(string resourceName)
        {
            return GetResource<TResource>(resourceName);
        }

        IEnumerable<IResource> IResourceProvider.Resources
        {
            get { return Resources; }
        }

        bool IResourceProvider.ContainsResource(string resourceName)
        {
            return ContainsResource(resourceName);
        }

        protected virtual bool ContainsResource(string resourceName)
        {
            return VisualState.ContainsResource(resourceName);
        }

        protected virtual TResource GetResource<TResource>(string resourceName)
            where TResource : class, IResource
        {
            return GetShape(resourceName) as TResource;
        }

        #endregion
    }
}