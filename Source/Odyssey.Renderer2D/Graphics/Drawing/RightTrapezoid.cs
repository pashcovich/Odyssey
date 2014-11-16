using System;
using Odyssey.Reflection;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Serialization;

namespace Odyssey.Graphics.Drawing
{
    public class RightTrapezoid : Path
    {
        internal float TopBaseRatio { get; private set; }

        protected override void OnInitializing(EventArgs e)
        {
            var d = new FigureDesigner();
            d.DrawRightTrapezoid(Position, Width*TopBaseRatio, Width, Height);
            Data = d.Result;
            base.OnInitializing(e);
        }

        protected internal override UIElement Copy()
        {
            var copy = (RightTrapezoid)base.Copy();
            copy.TopBaseRatio = TopBaseRatio;
            return copy;
        }

        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            base.OnReadXml(e);
            float r;
            TopBaseRatio = float.TryParse(e.XmlReader.GetAttribute(ReflectionHelper.GetPropertyName((RightTrapezoid t) => t.TopBaseRatio)), out r) ? r : 0.75f;
        }
    }
}
