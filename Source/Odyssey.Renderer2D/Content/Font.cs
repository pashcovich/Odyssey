#region Using Directives

using System.IO;
using SharpDX;

#endregion

namespace Odyssey.Content
{
    [ContentReader(typeof (FontReader))]
    public class Font
    {
        private readonly NativeFontFileStream nativeNativeFontFileStream;

        public Font(Stream fontStream)
        {
            var fontBytes = Utilities.ReadStream(fontStream);
            var stream = new DataStream(fontBytes.Length, true, true);
            stream.Write(fontBytes, 0, fontBytes.Length);
            stream.Position = 0;
            nativeNativeFontFileStream = new NativeFontFileStream(stream);
        }

        internal NativeFontFileStream NativeFontFileStream
        {
            get { return nativeNativeFontFileStream; }
        }
    }
}