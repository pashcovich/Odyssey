﻿using System.Xml;
using Odyssey.Content;

namespace Odyssey.Serialization
{
    public interface ISerializableResource
    {
        void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter);
        void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader);
    }
}
