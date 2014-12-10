#region Using Directives

using System;
using System.IO;
using Odyssey.Core;

#endregion

namespace Odyssey.Content
{
    /// <summary>
    ///     Parameters used by <see cref="IContentReader.ReadContent" />
    /// </summary>
    public struct ContentReaderParameters
    {
        /// <summary>
        ///     Name of the asset currently loaded when using <see cref="IAssetProvider.Load{T}" />.
        /// </summary>
        public string AssetName;

        /// <summary>
        ///     Type of the asset currently loaded when using <see cref="IAssetProvider.Load{T}" />.
        /// </summary>
        public Type AssetType;

        /// <summary>
        ///     This parameter is an out parameter for <see cref="IContentReader.ReadContent" />. Set to true to let the
        ///     ContentManager close the stream once the reader is done.
        /// </summary>
        public bool KeepStreamOpen;

        /// <summary>
        ///     Custom options provided when using <see cref="IAssetProvider.Load{T}" />.
        /// </summary>
        public object Options;

        public IServiceRegistry Services;

        /// <summary>
        ///     Stream of the asset to load.
        /// </summary>
        public Stream Stream;
    }
}