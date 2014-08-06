using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Odyssey.Content;
using Odyssey.Graphics.Shapes;

namespace Odyssey.UserInterface.Style
{
    [ContentReader(typeof(ThemeReader))]
    public class Theme : IXmlSerializable
    {
        private readonly Dictionary<string, ControlStyle> styles;
        private readonly Dictionary<string, Gradient> resources;
        public string Name { get; internal set; }
        public IEnumerable<ControlStyle> Styles { get { return styles.Values; }}

        public Theme()
        {
            styles = new Dictionary<string, ControlStyle>();
            resources= new Dictionary<string, Gradient>();
        }

        [Pure]
        public bool ContainsResource(string resourceName)
        {
            return resources.ContainsKey(resourceName);
        }

        public Gradient GetResource(string resourceName)
        {
            if (!ContainsResource(resourceName))
                throw new ArgumentException(string.Format("Resource '{0}' not found", resourceName));
            return resources[resourceName];
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            Name = reader.GetAttribute("Name");
            reader.ReadStartElement();
            while (reader.IsStartElement())
            {
                if (reader.Name == "ControlStyle")
                {
                    var style = new ControlStyle();
                    ((IXmlSerializable) style).ReadXml(reader);
                    styles.Add(style.Name, style);
                }
                else if (reader.Name == "Resources")
                {
                    while (reader.IsStartElement())
                    {
                        reader.ReadStartElement();
                        Type runTimeType;
                        switch (reader.Name)
                        {
                            default:
                                throw new InvalidOperationException(string.Format("Type '{0}' is not a valid Gradient",
                                    reader.Name));

                            case "LinearGradient":
                                runTimeType = typeof (LinearGradient);
                                break;

                            case "RadialGradient":
                                runTimeType = typeof (RadialGradient);
                                break;
                        }

                        var gradient = (Gradient) Activator.CreateInstance(runTimeType);
                        ((IXmlSerializable) gradient).ReadXml(reader);
                        resources.Add(gradient.Name, gradient);
                    }

                    reader.ReadEndElement();
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        public ControlStyle this[string key]
        {
            get { return styles[key]; }
        }
    }
}
