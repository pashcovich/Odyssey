using System;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using Odyssey.Engine;
using Odyssey.Utilities.Text;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public abstract class Gradient : IGradient, IXmlSerializable
    {
        public string Name { get; protected set; }

        public GradientType Type { get; protected set; }

        public GradientStopCollection GradientStops { get; private set; }

        protected Gradient() { }

        protected Gradient(string name, GradientStopCollection gradientStops, GradientType type)
        {
            this.Name = name;
            GradientStops = gradientStops;
            this.Type = type;
        }

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            OnReadXml(reader);
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            throw new System.NotImplementedException();
        }

        protected virtual void OnReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute("Name");
            if (string.IsNullOrEmpty(Name))
                throw new InvalidOperationException(string.Format("({0},{1}) 'Name' cannot be null", 0,0));
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

        internal abstract Gradient Copy();
    }
}