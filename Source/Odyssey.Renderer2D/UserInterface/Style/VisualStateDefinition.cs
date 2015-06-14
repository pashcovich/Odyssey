#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Odyssey.Animations;
using Odyssey.Content;
using Odyssey.Graphics.Drawing;
using Odyssey.Serialization;
using Odyssey.UserInterface.Controls;
using Odyssey.UserInterface.Data;

#endregion

namespace Odyssey.UserInterface.Style
{
    internal sealed class VisualStateDefinition : ISerializableResource, IResourceProvider
    {
        const string sShapes = "Shapes";
        const string sAnimations = "Animations";
        private const string sTriggers = "Triggers";

        private readonly List<Animation> animations;
        private readonly List<Shape> shapes;
        private readonly List<TriggerBase> triggers;

        public VisualStateDefinition()
        {
            shapes = new List<Shape>();
            animations = new List<Animation>();
            triggers = new List<TriggerBase>();
        }

        public IEnumerable<Shape> Shapes
        {
            get { return shapes; }
        }

        public IEnumerable<Animation> Animations
        {
            get { return animations; }
        }

        public IEnumerable<TriggerBase> Triggers
        {
            get { return triggers; }
        }

        #region ISerializableResource

        public void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader)
        {

            Name = xmlReader.GetAttribute("Name");

            if (!xmlReader.ReadToDescendant(sShapes))
                throw new InvalidOperationException(String.Format("{0}' element not found", sShapes));

            while (xmlReader.IsStartElement())
            {
                string elementName = xmlReader.Name;
                switch (elementName)
                {
                    case sAnimations:
                        DeserializeAnimations(resourceProvider, xmlReader);
                        break;

                    case sShapes:
                        DeserializeShapes(resourceProvider, xmlReader);
                        break;

                    case sTriggers:
                        DeserializeTriggers(resourceProvider, xmlReader);
                        break;
                }
            }

            xmlReader.ReadEndElement();
        }

        void DeserializeShapes(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            // Deserialize shapes
            xmlReader.ReadStartElement(sShapes);

            while (xmlReader.IsStartElement())
            {
                string typeName = string.Format("Odyssey.Graphics.Drawing.{0}", xmlReader.Name);
                try
                {
                    var shape = (Shape)Activator.CreateInstance(Type.GetType(typeName));
                    shape.DeserializeXml(resourceProvider, xmlReader);
                    shapes.Add(shape);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException(String.Format("Type '{0}' is not a valid Shape", typeName));
                }
                xmlReader.Read();
            }
            xmlReader.ReadEndElement();
        }

        void DeserializeAnimations(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            // Deserialize animations
            const string sAnimation = "Animation";
        
            xmlReader.ReadStartElement(sAnimations);
            while (xmlReader.IsStartElement(sAnimation))
            {
                string sState = xmlReader.GetAttribute("Name");
                ControlStatus cStatus;
                if (!Enum.TryParse(sState, out cStatus))
                    throw new InvalidOperationException(string.Format("'{0}' is not a valid state", sState));

                var animation = new Animation();
                animation.DeserializeXml(resourceProvider, xmlReader);
                animations.Add(animation);
            }
            xmlReader.ReadEndElement();
        }

        void DeserializeTriggers(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            // Deserialize triggers
            xmlReader.ReadStartElement(sTriggers);

            while (xmlReader.IsStartElement())
            {
                string typeName = string.Format("Odyssey.UserInterface.Data.{0}", xmlReader.Name);
                try
                {
                    var trigger = (TriggerBase)Activator.CreateInstance(Type.GetType(typeName));
                    trigger.DeserializeXml(resourceProvider, xmlReader);
                    triggers.Add(trigger);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException(String.Format("Type '{0}' is not a valid Trigger", typeName));
                }
                xmlReader.Read();
            }
            xmlReader.ReadEndElement();
        }

        #endregion

        #region IResourceProvider

        bool IResourceProvider.ContainsResource(string resourceName)
        {
            return Shapes.Any(s => s.Name == resourceName) || Animations.Any(a => a.Name == resourceName);
        }

        TResource IResourceProvider.GetResource<TResource>(string resourceName)
        {
            return Shapes.FirstOrDefault(s => s.Name == resourceName) as TResource ??
                   Animations.FirstOrDefault(s => s.Name == resourceName) as TResource;
        }

        IEnumerable<IResource> IResourceProvider.Resources
        {
            get
            {
                foreach (var shape in shapes)
                {
                    yield return shape;
                }
                foreach (var animation in animations)
                {
                    yield return animation;
                }
            }
        }

        #endregion

        public string Name { get; set; }
    }
}