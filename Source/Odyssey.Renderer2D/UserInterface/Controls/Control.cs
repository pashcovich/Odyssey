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
        private readonly string controlDescriptionClass;
        private ControlDescription description;
        private TextDescription textDescription;
        private string textDescriptionClass;

        protected Control(string descriptionClass, string textDescriptionClass = DefaultText)
        {
            controlDescriptionClass = descriptionClass;
            this.textDescriptionClass = textDescriptionClass;
        }

        protected IShape[] ActiveStyle { get; set; }

        protected ShapeMap ShapeMap { get; set; }

        #region Events

        /// <summary>
        /// Occurs when the <see cref="ControlDescription"/> property value changes.
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

        public string DescriptionClass
        {
            get { return controlDescriptionClass; }
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

        public string TextDescriptionClass
        {
            get { return textDescriptionClass; }
            set
            {
                if (string.Equals(textDescriptionClass, value))
                    return;

                textDescriptionClass = value;
                if (!DesignMode)
                    ApplyTextDescription();
            }
        }

        /// <summary>
        /// Gets or sets the <see cref = "Description" /> to use for this control.
        /// </summary>
        /// <value>The <see cref = "Description" /> object that contains information on how to style
        /// the text appearance of this control.</value>
        public ControlDescription Description
        {
            get { return description; }
            private set
            {
                if (description == value) return;

                description = value;
                OnControlDefinitionChanged(EventArgs.Empty);
            }
        }

        #endregion Properties

        #region Protected methods

        protected virtual void ApplyControlDescription()
        {
            var description = Overlay.StyleService.GetControlDescription(Overlay.ControlTheme, DescriptionClass);

            if (Width == 0 && Height == 0 && description.Size != default(Size2F))
            {
                Width = description.Size.Width;
                Height = description.Size.Height;
            }

            if (Padding.IsEmpty && !description.Padding.IsEmpty)
                Padding = description.Padding;

            TopLeftPosition = new Vector2(Padding.Left, Padding.Top);
            ShapeMap = new ShapeMap(description);
            LayoutUpdated += (s, e) => ShapeMap.Update();
            Description = description;
        }

        protected virtual void ApplyTextDescription()
        {
            var textDescription = Overlay.StyleService.GetTextDescription(Overlay.TextTheme, TextDescriptionClass);
            TextDescription = textDescription;
        }

        protected override void OnDesignModeChanged(ControlEventArgs e)
        {
            base.OnDesignModeChanged(e);
            if (ActiveStyle == null)
                return;

            foreach (UIElement element in ActiveStyle)
                element.DesignMode = e.Control.DesignMode;
        }

        protected override void OnInitialized(ControlEventArgs e)
        {
            base.OnInitialized(e);
            if (string.Equals(DescriptionClass, "Empty")) return;
            ShapeMap.Initialize();
            ActiveStyle = ShapeMap.GetShapes(ControlStatus.Enabled).ToArray();
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            base.OnInitializing(e);
            ApplyControlDescription();
            ApplyTextDescription();
        }

        #endregion Protected methods

        internal override UIElement Copy()
        {
            Control newControl = (Control) base.Copy();
            newControl.Description = Description;
            newControl.Padding = Padding;
            newControl.TextDescriptionClass = TextDescriptionClass;

            return newControl;
        }
    }
}