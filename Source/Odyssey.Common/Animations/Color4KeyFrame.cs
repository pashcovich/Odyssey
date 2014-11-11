using System;
using System.Xml.Linq;
using Odyssey.Graphics;
using Odyssey.Serialization;
using SharpDX.Mathematics;

namespace Odyssey.Animations
{
    public class Color4KeyFrame : KeyFrame<Color4>
    {
        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            base.OnReadXml(e);
            string value = e.XmlReader.GetAttribute("Value");
            string resourceName = Text.Text.ParseResource(value);
            if (!string.IsNullOrEmpty(resourceName))
                Value = e.ResourceProvider.GetResource<SolidColor>(resourceName).Color;
            else
                Value = Text.Text.DecodeColor4Abgr(value);

            e.XmlReader.ReadStartElement();
        }
    }
}
