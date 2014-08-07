using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Odyssey.Serialization;

namespace Odyssey.Animation
{
    public abstract class KeyFrame<T> : IComparable<KeyFrame<T>>, IKeyFrame, IStyleSerializable
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



        #region IStyleSerializable
        public void SerializeXml(Graphics.Shapes.IResourceProvider resourceProvider, XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(Graphics.Shapes.IResourceProvider resourceProvider, XmlReader reader)
        {
            OnReadXml(new XmlDeserializationEventArgs(resourceProvider, reader));
        }

        protected virtual void OnReadXml(XmlDeserializationEventArgs e)
        {
            string sTime = e.XmlReader.GetAttribute("Time");
            Time = TimeSpan.FromMilliseconds(double.Parse(sTime));
        }

        #endregion


    }
}