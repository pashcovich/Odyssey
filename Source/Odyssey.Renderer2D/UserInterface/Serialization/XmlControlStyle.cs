using System.Linq;
using Odyssey.Talos.Systems;
using Odyssey.UserInterface.Style;
using SharpDX;
using System;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;

namespace Odyssey.UserInterface.Xml
{
    /// <summary>
    /// Xml Wrapper class for the Control Style class.
    /// </summary>
    [XmlType(TypeName = "ControlStyle")]
    public struct XmlControlStyle
    {
        public XmlControlStyle(ControlStyle controlStyle) :
            this()
        {
            Contract.Requires<ArgumentNullException>(controlStyle != null, "ControlStyle is null");

            Name = controlStyle.Name;
            XmlMargin = StyleHelper.EncodeThickness(controlStyle.Margin);
            XmlPadding = StyleHelper.EncodeThickness(controlStyle.Padding);
            TextDescriptionClass = controlStyle.TextStyleClass;
        }

        public XmlVisualState VisualState { get; set; }

        #region Properties
        [XmlArray]
        [XmlArrayItem("LinearGradient", typeof(XmlGradient))]
        public XmlGradient[] Enabled { get; set; }

        [XmlArrayItem("LinearGradient", typeof(XmlGradient))]
        public XmlGradient[] Highlighted { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string TextDescriptionClass { get; set; }

        [XmlAttribute("Margin")]
        public string XmlMargin { get; set; }

        [XmlAttribute("Padding")]
        public string XmlPadding { get; set; }

        [XmlAttribute("Size")]
        public string XmlSize { get; set; }

        public ControlStyle ToControlDescription()
        {
            return new ControlStyle
            {
                Name = Name,
                Padding = String.IsNullOrEmpty(XmlPadding)
                    ? Thickness.Empty
                    : StyleHelper.DecodeThickness(XmlPadding),
                Margin = String.IsNullOrEmpty(XmlMargin)
                    ? Thickness.Empty
                    : StyleHelper.DecodeThickness(XmlMargin),
                Enabled = null,
                Highlighted = null,
                TextStyleClass = string.IsNullOrEmpty(TextDescriptionClass) ? "Default" : TextDescriptionClass
            };
        }

        #endregion Properties
    }

}