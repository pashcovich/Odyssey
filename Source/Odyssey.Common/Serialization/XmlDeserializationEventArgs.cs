using System;
using System.Xml;
using Odyssey.Graphics.Shapes;

namespace Odyssey.Serialization
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
