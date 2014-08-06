// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Engine;
using SharpDX;
using SharpDX.DirectWrite;

namespace Odyssey.Content
{
    /// <summary>
    ///     ResourceFont main loader. This classes implements FontCollectionLoader and FontFileLoader.
    ///     It reads all fonts embedded as resource in the current assembly and expose them.
    /// </summary>
    public class NativeFontLoader : CallbackBase, FontCollectionLoader, FontFileLoader
    {
        private readonly List<NativeFontFileEnumerator> _enumerators = new List<NativeFontFileEnumerator>();
        private readonly List<NativeFontFileStream> _fontStreams;
        private readonly DataStream _keyStream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NativeFontLoader" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        public NativeFontLoader(IServiceRegistry services)
        {
            var device = services.GetService<IDirect2DService>().Direct2DDevice;
            var factory = device.DirectWriteFactory;

            var content = services.GetService<IAssetProvider>();

            _fontStreams = content.GetAll<Font>().Select(f => f.NativeFontFileStream).ToList();

            // Build a Key storage that stores the index of the font
            _keyStream = new DataStream(sizeof (int)*_fontStreams.Count, true, true);
            for (int i = 0; i < _fontStreams.Count; i++)
                _keyStream.Write(i);
            _keyStream.Position = 0;

            // Register the 
            factory.RegisterFontFileLoader(this);
            factory.RegisterFontCollectionLoader(this);
        }


        /// <summary>
        ///     Gets the key used to identify the FontCollection as well as storing index for fonts.
        /// </summary>
        /// <value>The key.</value>
        public DataStream Key
        {
            get { return _keyStream; }
        }

        /// <summary>
        ///     Creates a font file enumerator object that encapsulates a collection of font files. The font system calls back to
        ///     this interface to create a font collection.
        /// </summary>
        /// <param name="factory">
        ///     Pointer to the <see cref="SharpDX.DirectWrite.Factory" /> object that was used to create the
        ///     current font collection.
        /// </param>
        /// <param name="collectionKey">
        ///     A font collection key that uniquely identifies the collection of font files within the
        ///     scope of the font collection loader being used. The buffer allocated for this key must be at least  the size, in
        ///     bytes, specified by collectionKeySize.
        /// </param>
        /// <returns>
        ///     a reference to the newly created font file enumerator.
        /// </returns>
        /// <unmanaged>
        ///     HRESULT IDWriteFontCollectionLoader::CreateEnumeratorFromKey([None] IDWriteFactory* factory,[In, Buffer]
        ///     const void* collectionKey,[None] int collectionKeySize,[Out] IDWriteFontFileEnumerator** fontFileEnumerator)
        /// </unmanaged>
        SharpDX.DirectWrite.FontFileEnumerator FontCollectionLoader.CreateEnumeratorFromKey(Factory factory, DataPointer collectionKey)
        {
            var enumerator = new NativeFontFileEnumerator(factory, this, collectionKey);
            _enumerators.Add(enumerator);

            return enumerator;
        }

        /// <summary>
        ///     Creates a font file stream object that encapsulates an open file resource.
        /// </summary>
        /// <param name="fontFileReferenceKey">
        ///     A reference to a font file reference key that uniquely identifies the font file
        ///     resource within the scope of the font loader being used. The buffer allocated for this key must at least be the
        ///     size, in bytes, specified by  fontFileReferenceKeySize.
        /// </param>
        /// <returns>
        ///     a reference to the newly created <see cref="SharpDX.DirectWrite.FontFileStream" /> object.
        /// </returns>
        /// <remarks>
        ///     The resource is closed when the last reference to fontFileStream is released.
        /// </remarks>
        /// <unmanaged>
        ///     HRESULT IDWriteFontFileLoader::CreateStreamFromKey([In, Buffer] const void* fontFileReferenceKey,[None] int
        ///     fontFileReferenceKeySize,[Out] IDWriteFontFileStream** fontFileStream)
        /// </unmanaged>
        SharpDX.DirectWrite.FontFileStream FontFileLoader.CreateStreamFromKey(DataPointer fontFileReferenceKey)
        {
            var index = SharpDX.Utilities.Read<int>(fontFileReferenceKey.Pointer);
            return _fontStreams[index];
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                foreach (var enumerator in _enumerators)
                {
                    ((FontFileEnumerator) enumerator).CurrentFontFile.Dispose();
                }
            }
        }
    }
}