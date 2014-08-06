using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Odyssey.Utilities.Text;
using SharpDX;

namespace Odyssey.Graphics.Shapes
{
    public class UniformGradient : Gradient
    {
        private Color4 color;
        public Color4 Color { get { return color; } }

        public UniformGradient(string name, Color4 color) : base(name, null, GradientType.Uniform)
        {
            this.color = color;
        }

        protected override void OnReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute("Name");
            color = Text.DecodeColor4Abgr(reader.GetAttribute("Color"));
            reader.Read();
            reader.ReadEndElement();
        }

        internal override Gradient Copy()
        {
            return new UniformGradient(Name, Color);
        }
    }
}
