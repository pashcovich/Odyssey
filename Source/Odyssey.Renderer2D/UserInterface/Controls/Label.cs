#region Using Directives

using Odyssey.Style;
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