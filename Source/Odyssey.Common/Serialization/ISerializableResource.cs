using System.Xml;
using Odyssey.Graphics;
using Odyssey.Graphics.Shapes;

namespace Odyssey.Serialization
{
    public interface ISerializableResource
    {
        void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter);
        void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader);
    }
}
