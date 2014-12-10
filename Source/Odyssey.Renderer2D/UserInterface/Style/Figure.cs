#region Using Directives

using System;
using System.Collections.Generic;
using System.Xml;
using Odyssey.Content;
using Odyssey.Reflection;
using Odyssey.Serialization;
using Odyssey.Text;

#endregion

namespace Odyssey.UserInterface.Style
{
    public sealed class Figure : IResource, ISerializableResource
    {
        public IEnumerable<VectorCommand> Data { get; private set; }
        public float ScaleTransformX { get; private set; }
        public float ScaleTransformY { get; private set; }
        public float StrokeThickness { get; private set; }
        public string Fill { get; private set; }
        public string Stroke { get; private set; }
        public string Name { get; private set; }

        public void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            Name = xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.Name));
            string pathData = xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.Data));

            if (string.IsNullOrEmpty(pathData))
                throw new InvalidOperationException(string.Format("No data specified for Figure '{0}'", Name));
            Data = VectorArtParser.ParsePathData(pathData);

            float scaleX;
            ScaleTransformX = float.TryParse(xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.ScaleTransformX)), out scaleX)
                ? scaleX
                : 1.0f;
            float scaleY;
            ScaleTransformY = float.TryParse(xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.ScaleTransformY)), out scaleY)
                ? scaleY
                : 1.0f;
            float strokeThickness;
            StrokeThickness = float.TryParse(xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.StrokeThickness)), out strokeThickness)
                ? strokeThickness
                : 0.0f;

            string sFill = xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.Fill));
            if (!string.IsNullOrEmpty(sFill))
                Fill = TextHelper.ParseResource(sFill);

            string sStroke = xmlReader.GetAttribute(ReflectionHelper.GetPropertyName((Figure f) => f.Stroke));
            if (!string.IsNullOrEmpty(sStroke))
                Stroke = TextHelper.ParseResource(sStroke);

            xmlReader.ReadStartElement();
        }
    }
}