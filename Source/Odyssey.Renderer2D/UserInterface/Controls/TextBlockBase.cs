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
using System.Diagnostics;
using Odyssey.Graphics;
using Odyssey.UserInterface.Events;
using Odyssey.UserInterface.Style;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics;
using Brush = Odyssey.Graphics.Brush;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;
using TextFormat = Odyssey.UserInterface.Style.TextFormat;

#endregion

namespace Odyssey.UserInterface.Controls
{
    [DebuggerDisplay("{Text} [{TextStyleClass}]")]
    public abstract class LabelBase : Control
    {
        protected const string DefaultTextClass = "Default";
        private const string ControlTag = "Default";
        private string text;
        private TextFormat textFormat;
        private TextLayout textLayout;

        protected TextLayout TextLayout { get { return textLayout; }}
        protected TextMetrics TextMetrics { get; private set; }

        protected LabelBase()
            : this(ControlTag)
        {
        }

        protected LabelBase(string textDefinitionClass)
            : base("Empty", textDefinitionClass)
        {
            CanRaiseEvents = false;
        }

        protected TextFormat TextFormat
        {
            get { return textFormat; }
            set { textFormat = value; }
        }

        public Brush Foreground { get; set; }

        public string Text
        {
            get { return text; }
            set
            {
                if (!string.Equals(text, value))
                {
                    string oldValue = text;
                    text = value;
                    OnTextChanged(new TextEventArgs(text, oldValue));
                }
            }
        }

        public event EventHandler<TextEventArgs> TextChanged;

        protected virtual void OnTextChanged(TextEventArgs e)
        {
            RaiseEvent(TextChanged, this, e);
        }

        protected override void OnInitializing(EventArgs e)
        {
            if (TextStyle == null)
                ApplyTextDescription();

            if (string.IsNullOrEmpty(Text))
                Text = Name;
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            var styleService = Overlay.Services.GetService<IStyleService>();
            if (Foreground == null)
            {
                var brushResource = Overlay.Theme.GetResource<ColorResource>(TextStyle.Foreground);
                Foreground = styleService.GetBrushResource(brushResource);
            }

            textFormat = styleService.GetTextResource(TextStyle);
            DeviceContext context = Device;
            context.TextAntialiasMode = TextAntialiasMode.Grayscale;

            if (DesignMode)
                return;
            InvalidateMeasure();
        }


        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            if (textLayout != null)
                RemoveAndDispose(ref textLayout);

            textLayout = ToDispose(new TextLayout(Device, Text, TextFormat, availableSizeWithoutMargins.X, availableSizeWithoutMargins.Y));
            TextMetrics = textLayout.Metrics;
            return new Vector3(TextMetrics.Width, TextMetrics.Height, availableSizeWithoutMargins.Z);
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            textLayout.MaxWidth = Width;
            textLayout.MaxHeight = Height;
        }

        public float MeasureText()
        {
            return TextMetrics.Width;
        }

        public float MeasureLineHeight()
        {
            return TextMetrics.Height;
        }

        public float MeasureText(int start, int length)
        {
            var metrics = textLayout.HitTestTextRange(start, length, 0, 0);
            return metrics[0].Width;
        }
    }
}