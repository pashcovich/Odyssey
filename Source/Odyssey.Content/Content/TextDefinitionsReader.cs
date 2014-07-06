using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.UserInterface.Style;
using Odyssey.UserInterface.Xml;

namespace Odyssey.Content
{
    [SupportedType(typeof(TextDescription[]))]
    public class TextDefinitionsReader : IResourceReader
    {
        public object ReadContent(IAssetProvider assetManager, string resourceName, System.IO.Stream stream)
        {
            return StyleManager.LoadDefinitions<XmlTextDescription>(stream).Select(xmlData => xmlData.ToTextDescription()).ToArray();
        }
    }
}
