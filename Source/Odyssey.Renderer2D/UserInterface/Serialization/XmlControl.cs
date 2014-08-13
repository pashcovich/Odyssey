using System.Text;
using System.Xml.Serialization;
using Odyssey.UserInterface.Controls;

namespace Odyssey.UserInterface.Xml
{
    public class XmlControl : XmlUIElement
    {
        public XmlControl()
        {
        }

        public XmlControl(Control control): base()
        {
            TextStyleClass = control.TextStyleClass;
            ControlStyleClass = control.StyleClass;
        }

        [XmlAttribute]
        public string ControlStyleClass { get; set; }

        [XmlAttribute]
        public string TextStyleClass { get; set; }

    }
}