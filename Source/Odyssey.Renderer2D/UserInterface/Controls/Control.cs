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

using Odyssey.Graphics;
using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Linq;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public abstract class Control : UIElement, IControl
    {
        public const string DefaultText = "Default";
        public const string EmptyStyle = "Empty";
        private readonly string controlStyleClass;
        private ControlStyle style;
        private TextDescription textDescription;
        private string textStyleClass;

        protected Control(string styleClass, string textStyleClass = DefaultText)
        {
            controlStyleClass = styleClass;
            this.textStyleClass = textStyleClass;
        }

        protected ControlStatus ActiveStatus { get; set; }

        protected VisualState VisualState { get; private set; }

        #region Events

        /// <summary>
        /// Occurs when the <see cref="ControlStyle"/> property value changes.
        /// </summary>
        public event EventHandler<EventArgs> ControlDefinitionChanged;

        /// <summary>
        /// Occurs when the <see cref="TextDefinition"/> property value changes.
        /// </summary>
        public event EventHandler<EventArgs> TextStyleChanged;

        /// <summary>
        /// Raises the <see cref="ControlDefinitionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnControlDefinitionChanged(EventArgs e)
        {
            if (!DesignMode)
                OnUpdate(e);

            RaiseEvent(ControlDefinitionChanged, this, e);
        }

        /// <summary>
        /// Raises the <see cref="TextStyleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event
        /// data.</param>
        protected virtual void OnTextDefinitionChanged(EventArgs e)
        {
            RaiseEvent(TextStyleChanged, this, e);
            if (!DesignMode)
                OnUpdate(e);
        }

        #endregion Events

        #region Properties

        public string StyleClass
        {
            get { return controlStyleClass; }
        }

        public Thickness Padding { get; set; }

        /// <summary>
        /// Gets or sets the <see cref = "TextDescription" /> to use for this control.
        /// </summary>
        /// <value>The <see cref = "TextDescription" /> object that contains information on how to
        /// format the text of this control.</value>
        public TextDescription TextDescription
        {
            get { return textDescription; }
            private set
            {
                if (textDescription == value) return;
                textDescription = value;
                OnTextDefinitionChanged(EventArgs.Empty);
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
                if (!DesignMode)
                    ApplyTextDescription();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref = "Style" /> to use for this control.
        /// </summary>
        /// <value>The <see cref = "Style" /> object that contains information on how to style
        /// the text appearance of this control.</value>
        public ControlStyle Style
        {
            get { return style; }
            private set
            {
                if (style == value) return;

                style = value;
                OnControlDefinitionChanged(EventArgs.Empty);
            }
        }

        #endregion Properties

        #region Protected methods

        protected virtual void ApplyControlDescription()
        {
            var controlStyle = Overlay.StyleService.GetControlStyle(Overlay.ControlTheme, StyleClass);

            if (Width == 0 && controlStyle.Width >0)
            {
                Width = controlStyle.Width;
            }

            if (Height == 0 && controlStyle.Height > 0)
            {
                Height = controlStyle.Height;
            }

            if (Padding.IsEmpty && !controlStyle.Padding.IsEmpty)
                Padding = controlStyle.Padding;

            TopLeftPosition = new Vector2(Padding.Left, Padding.Top);
            VisualState = controlStyle.VisualStateDefinition == null ? null : new VisualState(controlStyle.VisualStateDefinition);
            LayoutUpdated += (s, e) => VisualState.Update();
            Style = controlStyle;
        }

        protected virtual void ApplyTextDescription()
        {
            var textDescription = Overlay.StyleService.GetTextStyle(Overlay.TextTheme, TextStyleClass);
            TextDescription = textDescription;
        }

        protected override void OnDesignModeChanged(ControlEventArgs e)
        {
            base.OnDesignModeChanged(e);
            if (ActiveStatus == ControlStatus.None)
                return;

            foreach (Shape element in VisualState[ActiveStatus])
                element.DesignMode = e.Control.DesignMode;
        }

        protected override void OnInitialized(ControlEventArgs e)
        {
            base.OnInitialized(e);
            if (string.Equals(StyleClass, "Empty")) return;
            //ShapeMap.Initialize();
            VisualState.Initialize(this);
            ActiveStatus = IsEnabled ? ControlStatus.Enabled : ControlStatus.Disabled;
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            ApplyControlDescription();
            ApplyTextDescription();
        }

        #endregion Protected methods

        public override void Render()
        {
            foreach (IShape shape in VisualState[ActiveStatus])
                shape.Render();
        }

        internal override UIElement Copy()
        {
            Control newControl = (Control) base.Copy();
            newControl.Style = Style;
            newControl.Padding = Padding;
            newControl.TextStyleClass = TextStyleClass;

            return newControl;
        }
    }
}