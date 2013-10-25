using Odyssey.Engine;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Style
{
    public static class Extensions {

        public static TextFormat ToTextFormat(this Odyssey.UserInterface.Style.TextDescription tDesc, IDirectWriteProvider directWrite)
        {
            return new TextFormat(directWrite.Factory, tDesc.FontFamily, (FontWeight)tDesc.FontWeight, (FontStyle)tDesc.FontStyle, tDesc.Size)
            {
                TextAlignment = (TextAlignment)tDesc.TextAlignment,
                ParagraphAlignment = (ParagraphAlignment)tDesc.ParagraphAlignment,
            };
        }
    
    }
        
}
