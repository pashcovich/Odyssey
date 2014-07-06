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
    [XmlType(TypeName = "ControlDescription")]
    public struct XmlControlDescription
    {
        public XmlControlDescription(ControlDescription controlDescription) :
            this()
        {
            Contract.Requires<ArgumentNullException>(controlDescription != null, "controlDescription is null");

            Name = controlDescription.Name;
            XmlMargin = XmlCommon.EncodeThickness(controlDescription.Margin);
            BorderStyle = controlDescription.BorderStyle;
            XmlSize = XmlCommon.EncodeSize(controlDescription.Size);
            XmlPadding = XmlCommon.EncodeThickness(controlDescription.Padding);
            TextDescriptionClass = controlDescription.TextStyleClass;
        }

        #region Properties

        [XmlArray]
        [XmlArrayItem("LinearGradient", typeof(XmlGradient))]
        public XmlGradient[] BorderEnabled { get; set; }

        [XmlAttribute]
        public BorderStyle BorderStyle { get; set; }

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

        public ControlDescription ToControlDescription()
        {
            return new ControlDescription
                       {
                           Name = Name,
                           Size = String.IsNullOrEmpty(XmlSize) ? default(Size2F) : XmlCommon.DecodeSize(XmlSize),
                           Padding = String.IsNullOrEmpty(XmlPadding)
                                           ? Thickness.Empty
                                           : XmlCommon.DecodeThickness(XmlPadding),
                           BorderStyle = BorderStyle,
                           Margin = String.IsNullOrEmpty(XmlMargin)
                                           ? Thickness.Empty
                                           : XmlCommon.DecodeThickness(XmlMargin),
                           Enabled = Enabled != null
                                       ? Enabled[0].ToGradient()
                                       : null,
                           Highlighted = Highlighted != null
                               ? Highlighted[0].ToGradient()
                               : null,
                           BorderEnabled = BorderEnabled != null
                           ? BorderEnabled[0].ToGradient() : null,
                           TextStyleClass =
                                   string.IsNullOrEmpty(TextDescriptionClass) ? "Default" : TextDescriptionClass
                       };
        }

        private static IGradient ConvertGradient(XmlGradient gradient)
        {
            return gradient.ToGradient();
        }

        #endregion Properties
    }

    //[XmlType(TypeName = "TableStyle")]
    /*
    public class XmlTableStyle : XmlControlDescription
    {
        string xmlCellPadding;
        int cellSpacingX;
        int cellSpacingY;
        Border tableBorders;

        public XmlTableStyle(TableStyle tableStyle):
            base(tableStyle)
        {
            xmlCellPadding = XmlCommon.EncodeThickness(tableStyle.Cellpadding);
            cellSpacingX = tableStyle.CellSpacingX;
            cellSpacingY = tableStyle.CellSpacingY;
            tableBorders = tableStyle.TableBorders;
        }

        [XmlAttribute("CellPadding")]
        public string XmlCellPadding
        {
            get { return xmlCellPadding; }
            set { xmlCellPadding = value; }
        }

        [XmlAttribute]
        public int CellSpacingX
        {
            get { return cellSpacingX; }
            set { cellSpacingX = value; }
        }

        [XmlAttribute]
        public int CellSpacingY
        {
            get { return cellSpacingY; }
            set { cellSpacingY = value; }
        }

        public Border TableBorders
        {
            get { return tableBorders; }
            set { tableBorders = value; }
        }

        public TableStyle ToTableStyle()
        {
            TableStyle tableStyle = new TableStyle(
                Name,
                XmlCommon.DecodeSize(Size),
                XmlCommon.DecodeThickness(XmlPadding),
                XmlCommon.DecodeThickness(xmlCellPadding),
                cellSpacingX,
                cellSpacingY,
                tableBorders,
                BorderStyle,
                BorderSize,
                XmlGradientShader.ToShading(),
                XmlColorArray.ToColorArray());

            return tableStyle;
        }
    }
     * */
}