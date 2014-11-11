using System.Xml.Linq;
using Odyssey.Content;
using System;
using System.Collections.Generic;
using Odyssey.Reflection;
using Odyssey.Serialization;

namespace Odyssey.UserInterface.Style
{
    public sealed class Figure : IResource, ISerializableResource
    {
        public string Name { get; private set; }
        public IEnumerable<VectorCommand> Data { get; private set; }

        public void SerializeXml(IResourceProvider resourceProvider, System.Xml.XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(IResourceProvider resourceProvider, System.Xml.XmlReader xmlReader)
        {
            Name = xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.Name));
            string pathData = xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.Data));

            if (string.IsNullOrEmpty(pathData))
                throw new InvalidOperationException(string.Format("No data specified for Figure '{0}'", Name));
            Data = VectorArtParser.ParsePathData(pathData);
            xmlReader.ReadStartElement();
        }
    }
}
