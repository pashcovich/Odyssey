using Odyssey.Serialization;
using Odyssey.Utilities.Text;
using SharpDX;

namespace Odyssey.Graphics
{
    public class SolidColor : ColorResource
    {
        private Color4 color;
        public Color4 Color { get { return color; } }

        public SolidColor()
        { }

        public SolidColor(string name, Color4 color) : base(name,  GradientType.SolidColor)
        {
            this.color = color;
        }

        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            var reader = e.XmlReader;
            Name = reader.GetAttribute("Name");
            string colorValue = reader.GetAttribute("Color");
            color = Text.DecodeColor4Abgr(colorValue);
            reader.ReadStartElement();
        }
    }
}
