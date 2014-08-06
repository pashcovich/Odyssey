using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Odyssey.UserInterface.Serialization;
using Odyssey.UserInterface.Style;

namespace Odyssey.Content
{
    public class ThemeReader : IContentReader
    {
        public object ReadContent(IAssetProvider assetManager, ref ContentReaderParameters parameters)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Theme));
            Theme theme = new XmlStyleReader(parameters.Stream).Read();
            return theme;
        }
    }
}
