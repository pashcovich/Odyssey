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
using System.Diagnostics.Contracts;
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
    [DebuggerDisplay("[{GetType().Name}] {Text} [{TextStyleClass}]")]
    public abstract class TextBlockBase : VisualElement
    {
        protected const string DefaultTextClass = "Default";
        private const string ControlTag = "Default";
        private string text;
        private TextFormat textFormat;
        private TextLayout textLayout;

        protected TextLayout TextLayout { get { return textLayout; }}
        protected TextMetrics TextMetrics { get; private set; }

        protected TextBlockBase()
            : this(ControlTag)
        {
        }

        protected TextBlockBase(string textDefinitionClass)
            : base("Empty", textDefinitionClass)
        {
            CanRaiseEvents = false;
            Text = string.Empty;
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
            Contract.Requires<ArgumentNullException>(TextStyle != null || !string.IsNullOrEmpty(TextStyleClass), "TextStyleClasss");
            base.OnInitializing(e);

            if (string.IsNullOrEmpty(Text))
                Text = Name;

            var styleService = Overlay.Services.GetService<IStyleService>();
            textFormat = styleService.GetTextResource(TextStyle);
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            var styleService = Overlay.Services.GetService<IStyleService>();
            var brushResource = Overlay.Theme.GetResource<ColorResource>(TextStyle.Foreground);
            Foreground = styleService.GetBrushResource(brushResource, brushResource.Shared);
            textFormat = styleService.GetTextResource(TextStyle);
            DeviceContext context = Device;
            context.TextAntialiasMode = TextAntialiasMode.Grayscale;
        }

        protected override Vector3 MeasureOverride(Vector3 availableSizeWithoutMargins)
        {
            if (textLayout != null)
                RemoveAndDispose(ref textLayout);
            
            textLayout = ToDispose(new TextLayout(Device, Text, TextFormat, availableSizeWithoutMargins.X, availableSizeWithoutMargins.Y));
            TextMetrics = textLayout.Metrics;

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Stretch:
                    return new Vector3(availableSizeWithoutMargins.X, (float) Math.Ceiling(TextMetrics.Height), availableSizeWithoutMargins.Z);

                default:
                    return new Vector3((float) Math.Ceiling(TextMetrics.Width), (float) Math.Ceiling(TextMetrics.Height), availableSizeWithoutMargins.Z);
            }
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            textLayout.MaxWidth = e.NewSize.X;
            textLayout.MaxHeight = e.NewSize.Y;
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