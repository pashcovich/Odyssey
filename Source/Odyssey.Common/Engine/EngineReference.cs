#region Using Directives

using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Odyssey.Serialization;

#endregion

namespace Odyssey.Engine
{
    public sealed class EngineReference : IXmlSerializable, IDataSerializable
    {
        private static readonly IEqualityComparer<EngineReference> ComparerInstance = new ValueTypeEqualityComparer();
        private int index;
        private string type;
        private string value;

        public EngineReference() : this("Undefined", "Undefined")
        {
        }

        public EngineReference(string type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }

        public string Type
        {
            get { return type; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        internal static IEqualityComparer<EngineReference> Comparer
        {
            get { return ComparerInstance; }
        }

        #region IXmlSerializable

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            type = reader.GetAttribute("Type");
            value = reader.GetAttribute("Value");
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Type", Type);
            writer.WriteAttributeString("Value", Value);
        }

        #endregion

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref index);
            serializer.Serialize(ref type);
            serializer.Serialize(ref value);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}", Type, value);
        }

        private sealed class ValueTypeEqualityComparer : IEqualityComparer<EngineReference>
        {
            public bool Equals(EngineReference x, EngineReference y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.value, y.value) && string.Equals(x.type, y.type);
            }

            public int GetHashCode(EngineReference obj)
            {
                unchecked
                {
                    return ((obj.value != null ? obj.value.GetHashCode() : 0)*397) ^ (obj.type != null ? obj.type.GetHashCode() : 0);
                }
            }
        }
    }
}