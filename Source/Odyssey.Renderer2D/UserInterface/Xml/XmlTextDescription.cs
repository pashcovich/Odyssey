#region Using directives
using Odyssey.UserInterface.Style;
using SharpDX;
using System.Xml.Serialization;
#endregion Using directives

namespace Odyssey.UserInterface.Xml
{
    [XmlType(TypeName = "TextDescription")]
    public class XmlTextDescription
    {
        public XmlTextDescription()
        {
            Name = FontFamily = string.Empty;
            StandardColor = new XmlColor();
            TextAlignment = TextAlignment.Leading;
            ParagraphAlignment = ParagraphAlignment.Center;
            FontWeight = FontWeight.Normal;
            Size = 8;
        }

        public XmlTextDescription(TextDescription textDescription)
        {
            Name = textDescription.Name;

            IsOutlined = textDescription.IsOutlined;

            StandardColor = new XmlColor
                                {
                                    ColorValue = textDescription.Color.ToRgba().ToString("{0:X8}")
                                };
            HighlightedColor = new XmlColor
                                         {
                                             ColorValue = textDescription.HighlightedColor.ToRgba().ToString("{0:X8}")
                                         };
            FontFamily = textDescription.FontFamily;
            Size = textDescription.Size;
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool IsOutlined { get; set; }

        //[XmlAttribute]
        //public bool ApplyShadowing { get; set; }

        //[XmlAttribute]
        //public bool ApplyHighlight { get; set; }

        [XmlElement]
        public XmlColor StandardColor { get; set; }

        [XmlElement]
        public XmlColor HighlightedColor { get; set; }

        [XmlAttribute]
        public int Size { get; set; }

        [XmlAttribute]
        public string FontFamily { get; set; }

        [XmlAttribute]
        public TextAlignment TextAlignment { get; set; }

        [XmlAttribute]
        public ParagraphAlignment ParagraphAlignment { get; set; }

        [XmlAttribute]
        public FontStyle FontStyle { get; set; }
        [XmlAttribute]
        public FontWeight FontWeight { get; set; }

        public TextDescription ToTextDescription()
        {
            return new TextDescription
                       {
                           Name = Name,
                           Color = StandardColor.ToColor4(),
                           HighlightedColor = HighlightedColor.ColorValue != null ? HighlightedColor.ToColor4() : new Color4(),
                           FontFamily = FontFamily,
                           FontWeight = FontWeight,
                           FontStyle = FontStyle,
                           Size = Size,
                           TextAlignment = TextAlignment,
                           ParagraphAlignment = ParagraphAlignment,
                           IsOutlined = IsOutlined
                       };
        }
    }
}