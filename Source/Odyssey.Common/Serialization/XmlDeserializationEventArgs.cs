using System;
using System.Xml;
using Odyssey.Graphics;

namespace Odyssey.Serialization
{
    public class XmlDeserializationEventArgs : EventArgs
    {
        public IResourceProvider ResourceProvider { get; private set; }
        public XmlReader XmlReader { get; private set; }

        public XmlDeserializationEventArgs(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            ResourceProvider = resourceProvider;
            XmlReader = xmlReader;
        }
    }
}
