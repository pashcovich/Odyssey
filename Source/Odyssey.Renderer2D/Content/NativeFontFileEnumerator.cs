﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

using SharpDX;
using SharpDX.Mathematics;
using SharpDX.DirectWrite;

namespace Odyssey.Content
{
    /// <summary>
    /// Resource FontFileEnumerator.
    /// </summary>
    public class NativeFontFileEnumerator : CallbackBase, SharpDX.DirectWrite.FontFileEnumerator
    {
        private readonly Factory _factory;
        private readonly FontFileLoader _loader;
        private readonly DataStream keyStream;
        private FontFile _currentFontFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeFontFileEnumerator"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="loader">The loader.</param>
        /// <param name="key">The key.</param>
        public NativeFontFileEnumerator(Factory factory, FontFileLoader loader, DataPointer key)
        {
            _factory = factory;
            _loader = loader;
            keyStream = new DataStream(key.Pointer, key.Size, true, false);
        }

        /// <summary>
        /// Advances to the next font file in the collection. When it is first created, the enumerator is positioned before the first element of the collection and the first call to MoveNext advances to the first file.
        /// </summary>
        /// <returns>
        /// the value TRUE if the enumerator advances to a file; otherwise, FALSE if the enumerator advances past the last file in the collection.
        /// </returns>
        /// <unmanaged>HRESULT IDWriteFontFileEnumerator::MoveNext([Out] BOOL* hasCurrentFile)</unmanaged>
        bool SharpDX.DirectWrite.FontFileEnumerator.MoveNext()
        {
            bool moveNext = keyStream.RemainingLength != 0;
            if (moveNext)
            {
                if (_currentFontFile != null)
                    _currentFontFile.Dispose();

                _currentFontFile = new FontFile(_factory, keyStream.PositionPointer, 4, _loader);
                keyStream.Position += 4;
            }
            return moveNext;
        }

        /// <summary>
        /// Gets a reference to the current font file.
        /// </summary>
        /// <value></value>
        /// <returns>a reference to the newly created <see cref="SharpDX.DirectWrite.FontFile"/> object.</returns>
        /// <unmanaged>HRESULT IDWriteFontFileEnumerator::GetCurrentFontFile([Out] IDWriteFontFile** fontFile)</unmanaged>
        FontFile SharpDX.DirectWrite.FontFileEnumerator.CurrentFontFile
        {
            get
            {
                ((IUnknown)_currentFontFile).AddReference();
                return _currentFontFile;
            }
        }
    }
}