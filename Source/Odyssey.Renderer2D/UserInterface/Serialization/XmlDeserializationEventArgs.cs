using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Odyssey.Graphics;
using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Serialization
{
    public class XmlDeserializationEventArgs : EventArgs
    {
        public IResourceProvider Theme { get; private set; }
        public XmlReader XmlReader { get; private set; }

        public XmlDeserializationEventArgs(IResourceProvider theme, XmlReader xmlReader)
        {
            Theme = theme;
            XmlReader = xmlReader;
        }
    }
}
