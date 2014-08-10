using System;
using System.Xml;
using Odyssey.Serialization;
using Odyssey.Utilities.Text;
using SharpDX;

namespace Odyssey.Graphics
{
    public abstract class ColorResource : ISerializableResource, IResource
    {
        protected ColorResource() { }

        protected ColorResource(string name, GradientType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; protected set; }
        public GradientType Type { get; protected set; }

        public void SerializeXml(IResourceProvider theme, XmlWriter xmlWriter)
        {
            OnWriteXml(new XmlSerializationEventArgs(theme, xmlWriter));
        }

        public void DeserializeXml(IResourceProvider theme, XmlReader xmlReader)
        {
            OnReadXml(new XmlDeserializationEventArgs(theme, xmlReader));
        }

        protected virtual void OnReadXml(XmlDeserializationEventArgs e)
        {
            var reader = e.XmlReader;
            Name = reader.GetAttribute("Name");
            if (string.IsNullOrEmpty(Name))
            {
                IXmlLineInfo xmlInfo = (IXmlLineInfo)reader;
                throw new InvalidOperationException(string.Format("({0},{1}) 'Name' cannot be null", xmlInfo.LineNumber, xmlInfo.LinePosition));
            }
            
            reader.ReadStartElement();
        }

        protected virtual void OnWriteXml(XmlSerializationEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}