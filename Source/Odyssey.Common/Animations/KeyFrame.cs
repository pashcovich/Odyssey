using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Odyssey.Graphics;
using Odyssey.Serialization;

namespace Odyssey.Animations
{
    public abstract class KeyFrame<T> : IComparable<KeyFrame<T>>, IKeyFrame, ISerializableResource
    {
        public TimeSpan Time { get ; set; }
        public T Value { get; set; }

        public int CompareTo(KeyFrame<T> other)
        {
            return Time.CompareTo(other.Time);
        }

        object IKeyFrame.Value
        {
            get { return Value; }
            set { Value = (T)value; }
        }


        #region IStyleSerializable
        public void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            OnReadXml(new XmlDeserializationEventArgs(resourceProvider, xmlReader));
        }

        protected virtual void OnReadXml(XmlDeserializationEventArgs e)
        {
            string sTime = e.XmlReader.GetAttribute("Time");
            Time = TimeSpan.FromMilliseconds(double.Parse(sTime));
        }

        #endregion
    }
}