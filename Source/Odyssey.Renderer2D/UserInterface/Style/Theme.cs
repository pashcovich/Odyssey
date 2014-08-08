using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Odyssey.Animation;
using Odyssey.Content;
using Odyssey.Graphics;
using Odyssey.Graphics.Shapes;

namespace Odyssey.UserInterface.Style
{
    [ContentReader(typeof(ThemeReader))]
    public sealed class Theme : IXmlSerializable, IResourceProvider
    {
        const string sControlStyle = "ControlStyle";
        private const string sResources = "Resources";

        private readonly Dictionary<string, ControlStyle> styles;
        private readonly Dictionary<string, IResource> resources;

        public string Name { get; internal set; }

        public IEnumerable<ControlStyle> Styles { get { return styles.Values; }}

        public Theme()
        {
            styles = new Dictionary<string, ControlStyle>();
            resources = new Dictionary<string, IResource>();
        }

        [Pure]
        public bool ContainsResource(string resourceName)
        {
            return resources.ContainsKey(resourceName);
        }

        public IResource GetResource(string resourceName)
        {
            if (ContainsResource(resourceName))
                return resources[resourceName];
            else
            {
                foreach (var s in styles.Values)
                {
                    var r = s.FindResource(resourceName);
                    if (r != null)
                        return r;
                }
            }
            throw new ArgumentException(string.Format("Resource '{0}' not found", resourceName));
        }


        public ControlStyle GetStyle(string styleName)
        {
            if (!ContainsResource(styleName))
                throw new ArgumentException(string.Format("Style '{0}' not found", styleName));
            return styles[styleName];
        }

        public void AddResource(Gradient resource)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(resource.Name), "Resource name cannot be null");
            resources.Add(resource.Name, resource);
        }

        public void AddStyle(ControlStyle style)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(style.Name), "Style name cannot be null");
            styles.Add(style.Name, style);
        }

        #region IXmlSerializable
        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Name = reader.LocalName;
            reader.ReadStartElement();
            while (reader.IsStartElement())
            {
                if (reader.Name == sControlStyle)
                {
                    ParseControlStyles(reader);
                }
                else if (reader.Name == sResources)
                {
                    ParseResources(reader);
                }
            }
        }

        void ParseResources(XmlReader reader)
        {
            while (reader.IsStartElement())
            {
                reader.ReadStartElement();
                string typeName = String.Format("Odyssey.Graphics.Shapes.{0}, Odyssey.Common", reader.Name);
                try
                {
                    var gradient = (Gradient)Activator.CreateInstance(Type.GetType(typeName));
                    gradient.DeserializeXml(this, reader);
                    AddResource(gradient);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException(String.Format("Type '{0}' is not a valid Gradient", typeName));
                }
            }
            reader.ReadEndElement();
        }

        void ParseControlStyles(XmlReader reader)
        {
            while (reader.IsStartElement(sControlStyle))
            {
                var style = new ControlStyle();
                style.DeserializeXml(this, reader);
                
                AddStyle(style);
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        } 
        #endregion

        TResource IResourceProvider.GetResource<TResource>(string resourceName)
        {
            var resource = GetResource(resourceName) as TResource;
            if (resource == null)
                throw new InvalidCastException(string.Format("Resource '{0}' cannot be cast to {1}", resourceName, typeof(TResource).Name));
            return resource;
        }
    }
}
