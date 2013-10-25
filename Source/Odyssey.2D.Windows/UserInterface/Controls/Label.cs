using Odyssey.Engine;
using Odyssey.Graphics.Rendering2D.Shapes;
using Odyssey.Style;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.UserInterface.Controls
{
    public class Label : LabelBase
    {
        private SolidColorBrush textBrush;
        private TextFormat textFormat;

        public override void Initialize(IDirectXProvider directX)
        {
            IDirect2DProvider direct2D = directX.Direct2D;
            textBrush = ToDispose(new SolidColorBrush(direct2D.Context, TextDescription.Color));
            textFormat = ToDispose(TextDescription.ToTextFormat(directX.DirectWrite));
            direct2D.Context.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Grayscale;
            if (string.IsNullOrEmpty(Text))
                Text = Id;
        }

        public override void Render(Engine.IDirectXTarget target)
        {
            target.Direct2D.Context.DrawText(Text, textFormat, (RectangleF)Parent, textBrush);
        }
    }
}