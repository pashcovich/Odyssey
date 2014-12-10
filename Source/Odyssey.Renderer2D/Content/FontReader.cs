namespace Odyssey.Content
{
    public class FontReader : IContentReader
    {
        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            return new Font(parameters.Stream);
        }
    }
}