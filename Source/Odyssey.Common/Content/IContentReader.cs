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

namespace Odyssey.Content
{
    /// <summary>
    /// A content reader is in charge of reading object data from a stream.
    /// </summary>
    public interface IContentReader
    {
        /// <summary>
        /// Reads the content of a particular data from a stream.
        /// </summary>
        /// <param name="contentManager">The content manager.</param>
        /// <param name="parameters"></param>
        /// <returns>The data decoded from the stream, or null if the kind of asset is not supported by this content reader.</returns>
        object ReadContent(IAssetProvider contentManager, ref ContentReaderParameters parameters);
    }
}