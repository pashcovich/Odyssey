using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Odyssey.UserInterface.Style;

namespace Odyssey.UserInterface.Xml
{
    [XmlType("VisualStateDefinition")]
    public class XmlVisualState
    {
        [XmlArrayItem("Rectangle", typeof(XmlRectangle))]
        public XmlShape[] Shapes { get; set; }

        public XmlVisualState()
        {
        }

        public static implicit operator VisualState(XmlVisualState from)
        {
            return from == null ? null : new VisualState((from s in @from.Shapes select s.ToShape()));
        }
    }
}
