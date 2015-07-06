using System;
using System.Collections.Generic;
using System.Xml;
using Odyssey.Content;
using Odyssey.Organization.Commands;
using Odyssey.Reflection;
using Odyssey.Serialization;

namespace Odyssey.UserInterface.Data
{
    public class TriggerAction : ISerializableResource
    {
        public string CommandName { get; private set; }
        private readonly Dictionary<string, string> attributes;

        public TriggerAction()
        {
            attributes = new Dictionary<string, string>();
        }

        public void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            Type commandType = typeof (Command);
            string commandSfx = commandType.Name;
            string commandName = xmlReader.Name;
            Type actualCommandType = Type.GetType(string.Format("Odyssey.Organization.Commands.{0}{1}", commandName, commandSfx));
            if (!ReflectionHelper.IsTypeDerived(actualCommandType, commandType))
                throw new Exception(string.Format("Type {0} is not a valid {1}.", actualCommandType.Name, commandType.Name));

            CommandName = commandName;

            while (xmlReader.MoveToNextAttribute())
                attributes.Add(xmlReader.Name, xmlReader.Value);

            xmlReader.ReadStartElement();            
        }

        public bool HasKey(string key)
        {
            return attributes.ContainsKey(key);
        }

        public string this[String key] { get { return attributes[key]; }  }
    }
}
