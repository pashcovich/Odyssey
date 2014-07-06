using System.Linq;
using Odyssey.UserInterface.Style;
using Odyssey.UserInterface.Xml;

namespace Odyssey.Content
{
    public class TextDefinitionsReader : IContentReader
    {
        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            return StyleManager.LoadDefinitions<XmlTextDescription>(parameters.Stream).Select(xmlData => xmlData.ToTextDescription()).ToArray();
        }
    }
}
