using System.Linq;
using Odyssey.UserInterface.Style;
using Odyssey.UserInterface.Xml;

namespace Odyssey.Content
{
    public class ControlDefinitionsReader : IContentReader
    {
        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            return StyleManager.LoadDefinitions<ControlStyle>(parameters.Stream).ToArray();
        }
    }
}
