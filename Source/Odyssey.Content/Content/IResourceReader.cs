using System;
using System.IO;

namespace Odyssey.Content
{
    public interface IResourceReader
    {
        object ReadContent(IAssetProvider assetManager, string resourceName, Stream stream);
    }
}
