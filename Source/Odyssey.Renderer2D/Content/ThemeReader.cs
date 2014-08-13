using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Odyssey.UserInterface.Style;

namespace Odyssey.Content
{
    public class ThemeReader : IContentReader
    {
        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            var serializer = new XmlSerializer(typeof (Theme));
            var xmlReaderSettings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
            };
            Theme theme = (Theme)serializer.Deserialize(XmlReader.Create(parameters.Stream, xmlReaderSettings));
            return theme;
        }
    }
}
