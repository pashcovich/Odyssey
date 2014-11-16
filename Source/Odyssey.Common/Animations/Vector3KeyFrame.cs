using Odyssey.Serialization;
using SharpDX.Mathematics;

namespace Odyssey.Animations
{
    public class Vector3KeyFrame : KeyFrame<Vector3>
    {
        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            base.OnReadXml(e);
            string value = e.XmlReader.GetAttribute("Value");
            Value = Text.TextHelper.DecodeVector3(value);
            e.XmlReader.ReadStartElement();
        }
    }
}
