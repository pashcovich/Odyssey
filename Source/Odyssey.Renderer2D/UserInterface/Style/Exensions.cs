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

using SharpDX.DirectWrite;

#endregion Using Directives

namespace Odyssey.UserInterface.Style
{
    public static class Extensions
    {
        public static TextFormat ToTextFormat(this TextDescription tDesc, Factory factory)
        {
            return new TextFormat(factory, tDesc.FontFamily, (SharpDX.DirectWrite.FontWeight) tDesc.FontWeight,
                (SharpDX.DirectWrite.FontStyle) tDesc.FontStyle, tDesc.Size)
            {
                TextAlignment = (SharpDX.DirectWrite.TextAlignment) tDesc.TextAlignment,
                ParagraphAlignment = (SharpDX.DirectWrite.ParagraphAlignment) tDesc.ParagraphAlignment,
            };
        }
    }
}