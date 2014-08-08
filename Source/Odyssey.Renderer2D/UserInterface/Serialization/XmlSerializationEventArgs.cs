using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Odyssey.Graphics;
using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Serialization
{
    public class XmlSerializationEventArgs : EventArgs
    {
        public IResourceProvider Theme { get; private set; }
        public XmlWriter XmlWriter { get; private set; }

        public XmlSerializationEventArgs(IResourceProvider theme, XmlWriter xmlWriter)
        {
            Theme = theme;
            XmlWriter = xmlWriter;
        }
    }
}
