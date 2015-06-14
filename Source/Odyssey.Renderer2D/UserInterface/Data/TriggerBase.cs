using System.Xml;
using Odyssey.Content;
using Odyssey.Serialization;

namespace Odyssey.UserInterface.Data
{
    public abstract class TriggerBase : ISerializableResource
    {
        public abstract void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter);
        public abstract void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader);

        public abstract void Initialize(UIElement target);

    }
}
