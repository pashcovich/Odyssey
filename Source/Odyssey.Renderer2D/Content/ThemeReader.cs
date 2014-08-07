using System.Xml.Serialization;
using Odyssey.UserInterface.Style;

namespace Odyssey.Content
{
    public class ThemeReader : IContentReader
    {
        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            var serializer = new XmlSerializer(typeof (Theme));
            Theme theme = (Theme)serializer.Deserialize(parameters.Stream);
            return theme;
        }
    }
}
