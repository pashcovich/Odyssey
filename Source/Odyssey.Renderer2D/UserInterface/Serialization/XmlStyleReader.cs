using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Odyssey.Graphics.Shapes;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Style;
using SharpDX.Serialization;

namespace Odyssey.UserInterface.Serialization
{
    public class XmlStyleReader
    {
        private List<Gradient> resources;
        private readonly ControlStyle style ;
        private readonly XmlReader reader;

        public XmlStyleReader(Stream stream)
        {
            resources = new List<Gradient>();
            style = new ControlStyle();
            reader = XmlReader.Create(stream);
        }

        public Theme Read()
        {
            ParseVisualState();
            return null;
        }

        void ParseVisualState()
        {
            const string sShapes = "Shapes";
            const string sVisualState = "VisualState";
            style.Name = reader.GetAttribute("Name");

            string margin = reader.GetAttribute("Margin");
            style.Margin = string.IsNullOrEmpty(margin) ? Thickness.Empty : StyleHelper.DecodeThickness(margin);

            string textStyleClass = reader.GetAttribute("TextStyleClass");
            style.TextStyleClass = string.IsNullOrEmpty(textStyleClass) ? "Default" : textStyleClass;

            string padding = reader.GetAttribute("Padding");
            style.Padding = string.IsNullOrEmpty(padding) ? Thickness.Empty : StyleHelper.DecodeThickness(padding);

            string width = reader.GetAttribute("Width");
            string height = reader.GetAttribute("Height");
            style.Width = string.IsNullOrEmpty(width) ? 0 : float.Parse(width);
            style.Height = string.IsNullOrEmpty(height) ? 0 : float.Parse(height);

            List<Shape> shapes = new List<Shape>();

            if (!reader.ReadToDescendant(sVisualState))
                throw new InvalidOperationException(string.Format("{0}' element not found", sVisualState));

            if (!reader.ReadToDescendant(sShapes))
                throw new InvalidOperationException(string.Format("{0}' element not found", sShapes));

            // Advances reader to shape collection
            reader.ReadStartElement();

            while (reader.IsStartElement())
            {
                string type = reader.Name;
                try
                {
                    var shape = (Shape)Activator.CreateInstance(Type.GetType("Odyssey.Graphics.Shapes." + type));
                    ((IXmlSerializable)shape).ReadXml(reader);
                    shapes.Add(shape);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException(string.Format("Type '{0}' is not a valid Shape", type));
                }
                reader.Read();
            }
            reader.ReadEndElement();
            style.VisualStateDefinition = shapes;
        }

        private void ParseAnimations(XmlReader reader)
        {
            const string sAnimation = "Animation";
            const string sAnimationCurve = "AnimationCurve";
            string sStatus = reader.GetAttribute("Name");
            ControlStatus cStatus = (ControlStatus)Enum.Parse(typeof(ControlStatus), sStatus, true);
            reader.ReadStartElement();

            if (reader.IsStartElement(sAnimation))
                throw new InvalidOperationException(string.Format("{0} element not found", sAnimation));

            if (!reader.ReadToDescendant(sAnimationCurve))
                throw new InvalidOperationException(string.Format("{0} element not found", sAnimationCurve));

            //var animation = new Animation();
            //animation.N

            //while (reader.IsStartElement())
            //{
                
            //}

        }

        //void ParseShape(XmlReader reader)
        //{
        //    base.OnReadXml(reader);
        //    string strokeThickness = reader.GetAttribute("StrokeThickness");
        //    StrokeThickness = string.IsNullOrEmpty(strokeThickness) ? 0 : float.Parse(strokeThickness);
        //    string sFill = reader.GetAttribute("Fill");
        //    string sStroke = reader.GetAttribute("Stroke");
        //    Regex resourceRegex = new Regex(@"(?<=\{)\s*(?<name>\w*[^}]*)\s*(?=\})");
        //    if (!string.IsNullOrEmpty(sFill))
        //    {
        //        var match = resourceRegex.Match(sFill);
        //        FillGradientClass = match.Groups["name"].Value;
        //    }
        //    if (!string.IsNullOrEmpty(sStroke))
        //    {
        //        var match = resourceRegex.Match(sStroke);
        //        StrokeGradientClass = match.Groups["name"].Value;
        //    }
        //    if (!reader.IsEmptyElement)
        //        reader.ReadEndElement();
        //}
    }
}
