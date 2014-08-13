using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Style;
using Odyssey.Utilities.Text;
using SharpDX;
using System;
using System.Globalization;
using System.Xml.Serialization;

namespace Odyssey.UserInterface.Xml
{
    public struct XmlGradientStop
    {
        [XmlAttribute]
        public string Color { get; set; }

        [XmlAttribute]
        public float Offset { get; set; }

        public GradientStop ToGradientStop()
        {
            if (Color[0] == '#')
                Color = Color.Remove(0, 1);

            int abgrColor = Int32.Parse(Color, NumberStyles.HexNumber);
            int rgbaColor = Text.AbgrToRgba(abgrColor);

            return new GradientStop(new Color4(rgbaColor), Offset);
        }

        public XmlGradientStop(GradientStop gradientStop)
            : this()
        {
            Color = gradientStop.Color.ToRgba().ToString("X8");
            Offset = gradientStop.Offset;
        }
    }

    /// <summary>
    /// Xml Wrapper class for the LinearShader class.
    /// </summary>
    public abstract class XmlGradient
    {
        protected XmlGradient()
        { }

        protected XmlGradient(IGradient cs)
        {
            Name = cs.Name;
            if (cs.GradientStops == null) return;

            if (cs.GradientStops[0] == cs.GradientStops[1] || cs.GradientStops.Count == 1)
            {
                ColorValue = cs.GradientStops[0].Color.ToRgba().ToString("X8");
            }
            else
            {
                XmlGradientArray = new XmlGradientStop[cs.GradientStops.Count];
                for (int i = 0; i < cs.GradientStops.Count; i++)
                {
                    XmlGradientArray[i] = new XmlGradientStop(cs.GradientStops[i]);
                }
            }
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute("Color")]
        public string ColorValue { get; set; }

        [XmlArray("Gradient")]
        [XmlArrayItem("GradientStop")]
        public XmlGradientStop[] XmlGradientArray { get; set; }

        public abstract Gradient ToGradient();

    }

    [XmlType("LinearGradient")]
    public class XmlLinearGradient : XmlGradient
    {
        [XmlAttribute("StartPoint")]
        public string StartPointString { get; set; }
        [XmlAttribute("EndPoint")]
        public string EndPointString { get; set; }

        public XmlLinearGradient()
        {}

        public override Gradient ToGradient()
        {
            GradientStop[] gradientColors = null;

            if (XmlGradientArray != null)
            {
                gradientColors = new GradientStop[XmlGradientArray.Length];

                for (int i = 0; i < XmlGradientArray.Length; i++)
                {
                    gradientColors[i] = XmlGradientArray[i].ToGradientStop();
                }
            }
            else
            {
                gradientColors = new GradientStop[2];
                int rgbaColor = Int32.Parse(ColorValue, NumberStyles.HexNumber);
                gradientColors[0] = new GradientStop(new Color4(rgbaColor), 0);
                gradientColors[1] = gradientColors[0];
            }

            return new LinearGradient(Name, Text.DecodeVector2(StartPointString), Text.DecodeVector2(EndPointString), new GradientStopCollection(gradientColors));
        }
    }

    [XmlType("RadialGradient")]
    public class XmlRadialGradient : XmlGradient
    {
        public XmlRadialGradient()
        { }

        [XmlAttribute]
        public string Center { get; set; }

        [XmlAttribute]
        public float RadiusX { get; set; }

        [XmlAttribute]
        public float RadiusY { get; set; }

        public override Gradient ToGradient()
        {
            return null;
        }
    }

}