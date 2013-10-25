using Odyssey.UserInterface.Style;
using System;
using System.Xml.Serialization;

namespace Odyssey.UserInterface.Xml
{
    [XmlType("Border")]
    public class XmlBorderShader : XmlGradient
    {
        public XmlBorderShader()
        { }

        public XmlBorderShader(IBorderShader borderShader)
            : base(borderShader)
        {
            Borders = borderShader.Borders;
        }

        public XmlBorderShader(IGradient cs)
            : base(cs)
        { }

        [XmlIgnore]
        public Borders? Borders { get; set; }

        [XmlAttribute("Borders")]
        public string DoBstring
        {
            get
            {
                return Borders.HasValue
                ? Borders.ToString()
                : string.Empty;
            }
            set
            {
                Borders borders;
                Borders = Enum.TryParse(value, out borders) ? borders : Style.Borders.All;
            }
        }

        public override LinearGradient ToGradient()
        {
            LinearGradient cs = base.ToGradient();
            BorderShader bs = new BorderShader
            {
                Borders = Borders.HasValue ? Borders.Value : Style.Borders.All,
                GradientStops = cs.GradientStops,
                Type = cs.Type,
                Name = cs.Name
            };
            return bs;
        }
    }
}
