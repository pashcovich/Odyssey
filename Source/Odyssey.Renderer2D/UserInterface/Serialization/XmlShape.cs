using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Xml
{
    public abstract class XmlShape : XmlUIElement
    {
        [XmlElement("Fill")]
        public XmlGradient Fill { get; set; }
        public XmlGradient Stroke { get; set; }

        [XmlAttribute]
        public float StrokeThickness { get; set; }

        protected XmlShape()
        {}

        protected XmlShape(Shape shape) : base(shape)
        { }

        public abstract Shape ToShape();
    }

    [XmlType("Rectangle")]
    public class XmlRectangle : XmlShape
    {
        public XmlRectangle()
        {
        }

        public XmlRectangle(Rectangle rectangle) : base(rectangle)
        {
        }

        public override Shape ToShape()
        {
            return new Rectangle()
            {
                Position = Position,
                Width = Width,
                Height = Height,
                FillGradientClass = Fill.ToGradient(),
                StrokeGradientClass = Stroke.ToGradient(),
                StrokeThickness = StrokeThickness,
            };
        }

        

    }

}
