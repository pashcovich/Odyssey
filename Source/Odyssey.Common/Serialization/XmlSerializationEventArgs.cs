using System;
using System.Xml;
using Odyssey.Content;

namespace Odyssey.Serialization
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
