#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Odyssey.Content;
using Odyssey.Geometry.Primitives;
using Odyssey.Animation;
using Odyssey.Graphics;
using Odyssey.Graphics.Shapes;
using Odyssey.Serialization;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Serialization;
using Odyssey.UserInterface.Style;
using Odyssey.UserInterface.Xml;
using SharpDX;

#endregion

namespace Odyssey.UserInterface.Style
{
    public sealed class ControlStyle : ISerializableResource
    {
        private const string sShapes = "Shapes";
        private const string sVisualState = "VisualState";
        internal const string Error = "Error";
        internal const string Empty = "Empty";

        #region Properties

        public string Name { get; internal set; }
        public string TextStyleClass { get; internal set; }
        public float Width { get; internal set; }
        public float Height { get; internal set; }
        public Thickness Margin { get; internal set; }
        public Thickness Padding { get; internal set; }

        #endregion

        public IEnumerable<Shape> VisualStateDefinition { get; private set; }

        public IResource FindResource(string resourceName)
        {
            return VisualStateDefinition.FirstOrDefault(s => s.Name == resourceName);
        }

        public void DeserializeXml(IResourceProvider theme, XmlReader reader)
        {
            Name = reader.GetAttribute("Name");

            string margin = reader.GetAttribute("Margin");
            Margin = String.IsNullOrEmpty(margin) ? Thickness.Empty : StyleHelper.DecodeThickness(margin);

            string textStyleClass = reader.GetAttribute("TextStyleClass");
            TextStyleClass = String.IsNullOrEmpty(textStyleClass) ? "Default" : textStyleClass;

            string padding = reader.GetAttribute("Padding");
            Padding = String.IsNullOrEmpty(padding) ? Thickness.Empty : StyleHelper.DecodeThickness(padding);

            string width = reader.GetAttribute("Width");
            string height = reader.GetAttribute("Height");
            Width = String.IsNullOrEmpty(width) ? 0 : Single.Parse(width);
            Height = String.IsNullOrEmpty(height) ? 0 : Single.Parse(height);

            List<Shape> shapes = new List<Shape>();

            if (!reader.ReadToDescendant(sVisualState))
                throw new InvalidOperationException(String.Format("{0}' element not found", sVisualState));

            if (!reader.ReadToDescendant(sShapes))
                throw new InvalidOperationException(String.Format("{0}' element not found", sShapes));

            // Advances reader to shape collection
            reader.ReadStartElement();

            while (reader.IsStartElement())
            {
                string typeName = string.Format("Odyssey.Graphics.Shapes.{0}", reader.Name);
                try
                {
                    var shape = (Shape)Activator.CreateInstance(Type.GetType(typeName));
                    shape.DeserializeXml(theme, reader);
                    shapes.Add(shape);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException(String.Format("Type '{0}' is not a valid Shape", typeName));
                }
                reader.Read();
            }
            reader.ReadEndElement();
            VisualStateDefinition = shapes;
            ParseAnimations(theme, reader);
        }

        private void ParseAnimations(IResourceProvider theme, XmlReader reader)
        {
            const string sAnimation = "Animation";
            const string sAnimationCurve = "AnimationCurve";
            string sStatus = reader.GetAttribute("Name");
            ControlStatus cStatus = (ControlStatus)Enum.Parse(typeof(ControlStatus), sStatus, true);
            reader.ReadStartElement();

            if (!reader.IsStartElement(sAnimation))
                throw new InvalidOperationException(string.Format("{0} element not found", sAnimation));

            if (!reader.ReadToDescendant(sAnimationCurve))
                throw new InvalidOperationException(string.Format("{0} element not found", sAnimationCurve));

            var animationCurve = new AnimationCurve();
            animationCurve.DeserializeXml(theme, reader);

            //while (reader.IsStartElement())
            //{

            //}

        }


        public void SerializeXml(IResourceProvider theme, XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}