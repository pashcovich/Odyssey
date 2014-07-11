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

using Odyssey.UserInterface.Style;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode;

#endregion Using Directives

namespace Odyssey.UserInterface.Controls
{
    public class Label : LabelBase
    {
        private const string DefaultTextClass = "Default";
        private SolidColorBrush textBrush;
        private TextFormat textFormat;

        public Label()
            : base(DefaultTextClass)
        {
        }

        protected TextFormat TextFormat
        {
            get { return textFormat; }
            set { textFormat = value; }
        }

        public override bool Contains(Vector2 cursorLocation)
        {
            return BoundingRectangle.Contains(cursorLocation);
        }

        public override void Render()
        {
            DeviceContext context = Device;
            context.DrawText(Text, textFormat, BoundingRectangle, textBrush);
        }

        protected override void OnInitializing(ControlEventArgs e)
        {
            DeviceContext context = Device;
            if (TextDescription == default(TextDescription))
                ApplyTextDescription();

            textBrush = ToDispose(new SolidColorBrush(context, TextDescription.Color));
            textFormat = ToDispose(TextDescription.ToTextFormat(Device));
            context.TextAntialiasMode = TextAntialiasMode.Grayscale;
            if (string.IsNullOrEmpty(Text))
                Text = Name;
        }
    }
}