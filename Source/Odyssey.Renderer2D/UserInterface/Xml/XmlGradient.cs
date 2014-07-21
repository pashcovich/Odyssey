using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Style;
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
            int rgbaColor = XmlCommon.AbgrToRgba(abgrColor);

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
    [XmlType("Gradient")]
    public class XmlGradient
    {
        public XmlGradient()
        { }

        public XmlGradient(IGradient cs)
        {
            Name = cs.Name;
            GradientType = cs.Type;
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

        [XmlAttribute]
        public GradientType GradientType { get; set; }

        [XmlAttribute("Color")]
        public string ColorValue { get; set; }

        [XmlArray("Gradient")]
        [XmlArrayItem("GradientStop")]
        public XmlGradientStop[] XmlGradientArray { get; set; }

        public virtual LinearGradient ToGradient()
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
            Type shaderType;
            switch (GradientType)
            {
                //case GradientType.Radial:
                //    shaderType = typeof(RadialShader);
                //    break;

                default:
                    shaderType = typeof(LinearGradient);
                    break;
            }

            return new LinearGradient(Name,Vector2.Zero, new Vector2(1,0), new GradientStopCollection(gradientColors));
        }
    }
    /*
    

    [XmlType("Radial")]
    public class XmlRadialShader : XmlGradient
    {
        public XmlRadialShader()
        { }

        public XmlRadialShader(RadialShader radialShader)
            : base(radialShader)
        {
            Center = XmlCommon.EncodeVector2(radialShader.Center);
            GradientType = radialShader.GradientType;
            RadiusX = radialShader.RadiusX;
            RadiusY = radialShader.RadiusY;
        }

        [XmlAttribute]
        public string Center { get; set; }

        [XmlAttribute]
        public float RadiusX { get; set; }

        [XmlAttribute]
        public float RadiusY { get; set; }

        public override LinearShader ToColorShader()
        {
            const float defaultValue = 0.5f;
            Vector2 defaultCenter = new Vector2(defaultValue, defaultValue);
            LinearShader linearShader = base.ToColorShader();
            RadialShader radialShader = new RadialShader
            {
                Center = string.IsNullOrEmpty(Center) ? defaultCenter : XmlCommon.DecodeFloatVector2(Center),
                RadiusX = RadiusX == 0 ? defaultValue : RadiusX,
                RadiusY = RadiusY == 0 ? defaultValue : RadiusY,
                Gradient = linearShader.Gradient,
                GradientType = linearShader.GradientType,
                Method = linearShader.Method,
                Name = linearShader.Name
            };
            return radialShader;
        }
    }
     * 
     */
}