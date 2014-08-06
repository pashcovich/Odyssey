using System;
using System.Xml;
using System.Xml.Serialization;

namespace Odyssey.Animation
{
    public abstract class KeyFrame<T> : IComparable<KeyFrame<T>>, IKeyFrame, IXmlSerializable
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

        protected static float Map(TimeSpan start, TimeSpan end, TimeSpan value)
        {
            float low = (float)(start.TotalMilliseconds);
            float high = (float)(end.TotalMilliseconds);
            float time = (float)(value.TotalMilliseconds);
            return (time - low) / (high - low);
        }

        protected virtual void OnReadXml(XmlReader reader)
        {
            
        }

        #region IXmlSerializable
        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}