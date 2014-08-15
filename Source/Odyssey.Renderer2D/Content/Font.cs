using System.IO;
using SharpDX;

namespace Odyssey.Content
{
    [ContentReader(typeof(FontReader))]
    public class Font
    {
        private readonly NativeFontFileStream nativeNativeFontFileStream;

        internal NativeFontFileStream NativeFontFileStream
        {
            get { return nativeNativeFontFileStream; }
        }

        public Font(Stream fontStream)
        {
            var fontBytes = SharpDX.Utilities.ReadStream(fontStream);
            var stream = new DataStream(fontBytes.Length, true, true);
            stream.Write(fontBytes, 0, fontBytes.Length);
            stream.Position = 0;
            this.nativeNativeFontFileStream = new NativeFontFileStream(stream);
        }
    }
}
