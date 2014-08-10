using Odyssey.Serialization;
using Odyssey.Utilities.Text;
using SharpDX;

namespace Odyssey.Graphics
{
    public abstract class Gradient : ColorResource, IGradient
    {
        public GradientStopCollection GradientStops { get; private set; }

        protected Gradient() {}

        protected Gradient(string name, GradientStopCollection gradientStops, GradientType type)
            : base(name, type)
        {
            Name = name;
            GradientStops = gradientStops;
            Type = type;
        }
        
        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            base.OnReadXml(e);
            var reader = e.XmlReader;
            GradientStops = new GradientStopCollection();
            while (reader.IsStartElement("GradientStop"))
            {
                string colorValue = reader.GetAttribute("Color");

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
    }
}