using System.Xml;
using Odyssey.Graphics.Shapes;

namespace Odyssey.Serialization
{
    public interface IStyleSerializable
    {
        void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter);
        void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader);
    }
}
