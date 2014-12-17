#region Using Directives

using Odyssey.Reflection;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Serialization;
using SharpDX.Mathematics;

#endregion

namespace Odyssey.Graphics.Drawing
{
    public class RightTrapezoid : Path
    {
        internal float TopBaseRatio { get; private set; }

        protected internal override UIElement Copy()
        {
            var copy = (RightTrapezoid) base.Copy();
            copy.TopBaseRatio = TopBaseRatio;
            return copy;
        }

        protected override void OnReadXml(XmlDeserializationEventArgs e)
        {
            base.OnReadXml(e);
            float r;
            TopBaseRatio =float.TryParse(e.XmlReader.GetAttribute(ReflectionHelper.GetPropertyName((RightTrapezoid t) => t.TopBaseRatio)), out r) ? r : 0.75f;
        }

        protected override Vector3 ArrangeOverride(Vector3 availableSizeWithoutMargins)
        {
            var d = new FigureDesigner();
            d.DrawRightTrapezoid(Position, availableSizeWithoutMargins.X * TopBaseRatio, availableSizeWithoutMargins.X, availableSizeWithoutMargins.Y);
            Data = d.Result;
            CreatePathGeometry();
            return availableSizeWithoutMargins;
        }

    }
}