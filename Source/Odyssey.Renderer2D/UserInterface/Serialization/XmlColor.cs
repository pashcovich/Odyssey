using SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Odyssey.UserInterface.Xml
{
    /// <summary>
    /// Xml Wrapper class for the System.Drawing.Color class.
    /// </summary>
    [XmlType(TypeName = "Color")]
    public struct XmlColor
    {
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("HexValue")]
        public string ColorValue { get; set; }

        public Color4 ToColor4()
        {
            int rgbaColor = Int32.Parse(ColorValue, NumberStyles.HexNumber);
            return new Color4(rgbaColor);
        }
    }
}
