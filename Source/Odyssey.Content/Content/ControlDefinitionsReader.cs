using System.Linq;
using Odyssey.UserInterface.Style;
using Odyssey.UserInterface.Xml;

namespace Odyssey.Content
{
    [SupportedType(typeof(ControlDescription[]))]
    public class ControlDefinitionsReader : IResourceReader
    {
        public object ReadContent(IAssetProvider assetManager, string resourceName, System.IO.Stream stream)
        {
            return StyleManager.LoadDefinitions<XmlControlDescription>(stream).Select(xmlData => xmlData.ToControlDescription()).ToArray();
        }
    }
}
