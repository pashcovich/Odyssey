#region Using Directives

using Odyssey.Reflection;
using Odyssey.UserInterface;
using Odyssey.UserInterface.Serialization;

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

        protected override void Redraw()
        {
            var d = new FigureDesigner();
            d.DrawRightTrapezoid(Position, RenderSize.X*TopBaseRatio, RenderSize.X, RenderSize.Y);
            Data = d.Result;
            base.Redraw();
        }
    }
}