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
        public float ScaleTransformX { get; set; }
        public float ScaleTransformY { get; set; }

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

            float scaleX;
            ScaleTransformX = float.TryParse(xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.ScaleTransformX)), out scaleX) ? scaleX : 1.0f;
            float scaleY;
            ScaleTransformY = float.TryParse(xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.ScaleTransformY)), out scaleY) ? scaleY : 1.0f;
            xmlReader.ReadStartElement();
        }
    }
}
