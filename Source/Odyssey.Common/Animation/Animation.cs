using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Odyssey.Engine;

namespace Odyssey.Animation
{
    public class Animation : IXmlSerializable
    {
        readonly Dictionary<string, IAnimationCurve> animations;
        public float Duration { get { return animations.Values.Max(a => a.Duration); } }
        public WrapMode WrapMode { get; set; }

        public Animation()
        {
            animations = new Dictionary<string, IAnimationCurve>();
        }

        [Pure]
        public bool Contains(IAnimationCurve animationCurve)
        {
            Contract.Requires<ArgumentNullException>(animationCurve != null, "animationCurve");
            return animations.ContainsKey(animationCurve.Name);
        }

        public void AddCurve(IAnimationCurve curve)
        {
            Contract.Requires<ArgumentException>(!Contains(curve), "curve");
            animations.Add(curve.Name, curve);
        }

        public void Update(ITimeService time, object obj)
        {
            foreach (var curve in animations.Values)
                curve.Update(time, obj);
        }

        protected virtual void OnReadXml(XmlReader reader)
        {
            while (reader.IsStartElement())
            {
                var animationCurve = new AnimationCurve();
            }
        }

        #region IXmlSerializable
        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            OnReadXml(reader);
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
