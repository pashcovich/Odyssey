#region Using Directives

using Odyssey.Graphics;
using Odyssey.Serialization;
using Odyssey.Text;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.Animations
{
    public class Color4KeyFrame : KeyFrame<Color4>
    {
        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            base.OnReadXml(e);
            string value = e.XmlReader.GetAttribute("Value");
            string resourceName = TextHelper.ParseResource(value);
            if (!string.IsNullOrEmpty(resourceName))
                Value = e.ResourceProvider.GetResource<SolidColor>(resourceName).Color;
            else
                Value = TextHelper.DecodeColor4Abgr(value);

            e.XmlReader.ReadStartElement();
        }
    }
}