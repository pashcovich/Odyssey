using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Odyssey.Engine;
using Odyssey.Serialization;
using Odyssey.Utilities.Text;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public abstract class Gradient : IGradient, IStyleSerializable
    {
        public string Name { get; protected set; }

        public GradientType Type { get; protected set; }

        public GradientStopCollection GradientStops { get; private set; }

        protected Gradient() { }

        protected Gradient(string name, GradientStopCollection gradientStops, GradientType type)
        {
            Name = name;
            GradientStops = gradientStops;
            Type = type;
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
            GradientStops = new GradientStopCollection();
            reader.ReadStartElement();
            
            while (reader.IsStartElement("GradientStop"))
            {
                string colorValue = reader.GetAttribute("Color");
                if (!string.IsNullOrEmpty(colorValue) && colorValue[0] == '#')
                    colorValue = colorValue.Remove(0, 1);

                string offset = reader.GetAttribute("Offset");
                var gradientStop = new GradientStop()
                {
                    Color = string.IsNullOrEmpty(colorValue) ? new Color4(0, 0, 0, 0) : Text.DecodeColor4Abgr(colorValue),
                    Offset = string.IsNullOrEmpty(offset) ? 0 : float.Parse(offset)
                };
                reader.Read();
                GradientStops.Add(gradientStop);
            }

            reader.ReadEndElement();
        }

        protected virtual void OnWriteXml(XmlSerializationEventArgs e)
        {
            throw new NotImplementedException();
        }

        internal abstract Gradient Copy();

        public void SerializeXml(IResourceProvider theme, XmlWriter xmlWriter)
        {
            OnWriteXml(new XmlSerializationEventArgs(theme, xmlWriter));
        }

        public void DeserializeXml(IResourceProvider theme, XmlReader xmlReader)
        {
            OnReadXml(new XmlDeserializationEventArgs(theme, xmlReader));
        }
    }
}