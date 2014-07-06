using Odyssey.UserInterface.Controls;
using System;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace Odyssey.UserInterface.Xml
{
    //[XmlType(TypeName = "Label")]
    //public class XmlLabel : XmlBaseControl
    //{
    //    string text;

    // public XmlLabel() : base() { text = string.Empty; }

    // [XmlAttribute] public string Text { get { return text; } set { text = value; } }

    // public override void FromControl(BaseControl control) { base.FromControl(control); Label
    // label = control as Label; text = label.Text; }
    //}

    //[XmlType(TypeName = "Button")]
    //public class XmlButton : XmlBaseControl
    //{
    //    public XmlButton(Button button)
    //        : base(button)
    //    {
    //        Content = button.Content;
    //    }

    // [XmlAttribute] public string Content { get; set; }

    // protected override void WriteCustomCsCode(StringBuilder sb) { sb.AppendFormat("\t\tContent =
    // {0},\n", Content); }
    //}

    //[XmlType(TypeName = "DecoratedButton")]
    //public class XmlDecoratedButton : XmlBaseControl
    //{
    //    DecorationType decorationType;

    // public XmlDecoratedButton() : base() { decorationType = DecorationType.DownsideTriangle; }

    // [XmlAttribute] public DecorationType DecorationType { get { return decorationType; } set {
    // decorationType = value; } }

    // public override void FromControl(BaseControl control) { base.FromControl(control);
    // DecoratedButton decoratedButton = control as DecoratedButton; decorationType =
    // decoratedButton.DecorationType; }
    //}

    //[XmlType(TypeName = "TextBox")]
    //public class XmlTextBox : XmlBaseControl
    //{
    //    bool acceptsReturn;
    //    KeyType keyMask;
    //    string text;

    // public XmlTextBox() : base() { text = string.Empty; }

    // [XmlAttribute] public string Text { get { return text; } set { text = value; } }

    // [XmlAttribute] public bool AcceptsReturn { get { return acceptsReturn; } set { acceptsReturn
    // = value; } }

    // [XmlAttribute] public KeyType KeyMask { get { return keyMask; } set { keyMask = value; } }

    // public override void FromControl(BaseControl control) { base.FromControl(control); TextBox
    // textBox = control as TextBox; text = textBox.Text; TextDescriptionClass =
    // textBox.TextStyle.Name; acceptsReturn = textBox.AcceptsReturn; keyMask = textBox.KeyMask; }
    //}

    //[XmlType(TypeName = "TrackBar")]
    //public class XmlTrackBar : XmlBaseControl
    //{
    //    int maxValue;
    //    int minValue;
    //    int tickFrequency;

    // public XmlTrackBar() : base() { }

    // [XmlAttribute] public int MinimumValue { get { return minValue; } set { minValue = value; } }

    // [XmlAttribute] public int MaximumValue { get { return maxValue; } set { maxValue = value; } }

    // [XmlAttribute] public int TickFrequency { get { return tickFrequency; } set { tickFrequency =
    // value; } }

    // public override void FromControl(BaseControl control) { base.FromControl(control); TrackBar
    // trackBar = control as TrackBar; minValue = trackBar.MinimumValue; maxValue =
    // trackBar.MaximumValue; tickFrequency = trackBar.TickFrequency; }
    //}

    //[XmlType(TypeName ="CheckBox")]
    //public class XmlCheckBox : XmlBaseControl
    //{
    //    bool isChecked;

    // public XmlCheckBox() : base() { }

    // [XmlAttribute] public bool IsChecked { get { return isChecked; } set { isChecked = value; } }

    // public override void FromControl(BaseControl control) { base.FromControl(control); CheckBox
    // checkBox = control as CheckBox; TextDescriptionClass = checkBox.TextStyle.Name; isChecked =
    // checkBox.IsChecked; } }

    public class XmlUnknownControl : XmlBaseControl
    {
        protected override void WriteCustomCsCode(StringBuilder sb)
        {
            return;
        }
    }
}