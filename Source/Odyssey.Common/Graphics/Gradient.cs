#region Using Directives

using System.Collections.Generic;
using System.Globalization;
using Odyssey.Serialization;
using Odyssey.Text;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.Graphics
{
    public abstract class Gradient : ColorResource, IGradient
    {
        private readonly GradientStopCollection gradientStops;

        protected Gradient(string name, IEnumerable<GradientStop> gradientStops, ExtendMode extendMode, ColorType type, float opacity, bool shared)
            : base(name, type, opacity, shared)
        {
            this.gradientStops = new GradientStopCollection(gradientStops);
            this.gradientStops.ExtendMode = extendMode;
        }

        public GradientStopCollection GradientStops
        {
            get { return gradientStops; }
        }

        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            base.OnReadXml(e);
            var reader = e.XmlReader;
            reader.ReadStartElement();
            while (reader.IsStartElement("GradientStop"))
            {
                Color4 color;
                string colorValue = reader.GetAttribute("Color");
                string colorAsResource = TextHelper.ParseResource(colorValue);
                if (!string.IsNullOrEmpty(colorAsResource))
                    color = e.ResourceProvider.GetResource<SolidColor>(colorAsResource).Color;
                else
                    color = string.IsNullOrEmpty(colorValue) ? new Color4(0, 0, 0, 0) : TextHelper.DecodeColor4Abgr(colorValue);

                string offset = reader.GetAttribute("Offset");
                var gradientStop = new GradientStop
                {
                    Color = color,
                    Offset = string.IsNullOrEmpty(offset) ? 0 : float.Parse(offset, CultureInfo.InvariantCulture)
                };
                reader.ReadStartElement();
                GradientStops.Add(gradientStop);
            }

            reader.ReadEndElement();
        }
    }
}