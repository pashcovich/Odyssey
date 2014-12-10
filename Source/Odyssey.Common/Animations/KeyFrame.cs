#region Using Directives

using System;
using System.Xml;
using Odyssey.Content;
using Odyssey.Serialization;

#endregion

namespace Odyssey.Animations
{
    public abstract class KeyFrame<T> : IComparable<KeyFrame<T>>, IKeyFrame, ISerializableResource
    {
        public T Value { get; set; }

        public int CompareTo(KeyFrame<T> other)
        {
            return Time.CompareTo(other.Time);
        }

        public float Time { get; set; }

        object IKeyFrame.Value
        {
            get { return Value; }
            set { Value = (T) value; }
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
            Time = float.Parse(sTime);
        }

        #endregion
    }
}