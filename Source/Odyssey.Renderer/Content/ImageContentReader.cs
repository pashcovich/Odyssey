using Odyssey.Graphics;

namespace Odyssey.Content
{
    /// <summary>
    /// Content reader for an image.
    /// </summary>
    class ImageContentReader : IContentReader
    {
        public object ReadContent(IAssetProvider readerManager, ref ContentReaderParameters parameters)
        {
            parameters.KeepStreamOpen = false;
            var image = Image.Load(parameters.Stream);
            if (image != null)
                image.Name = parameters.AssetName;
            return image;
        }
    }
}
