using System.Text.RegularExpressions;
using System.Xml;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Serialization;
using Odyssey.Utilities.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Odyssey.Utilities.Text;

namespace Odyssey.Animations
{
    public abstract class AnimationCurve<TKeyFrame> : ISerializableResource, IResource, IAnimationCurve, IEnumerable<TKeyFrame> 
        where TKeyFrame : class, IKeyFrame
    {
        public delegate object CurveFunction(TKeyFrame start, TKeyFrame end, TimeSpan time);

        private readonly List<TKeyFrame> keyFrames;

        private TimeSpan elapsedTime;
        private object target;

        public int Length { get { return keyFrames.Count; } }

        /// <inheritdoc/>
        public float Duration { get { return (float)keyFrames.Max().Time.TotalSeconds; } }
        public string TargetProperty { get; internal set; }
        public string TargetName { get; internal set; }
        public string Name { get; set; }

        public CurveFunction Function { get; set; }

        public AnimationCurve()
        {
            keyFrames = new List<TKeyFrame>();
        }

        public void AddKeyFrame(TKeyFrame keyFrame)
        {
            Contract.Requires<ArgumentNullException>(keyFrame != null, "keyFrame");
            keyFrames.Add(keyFrame);
            keyFrames.Sort();
        }

        public void Clear()
        {
            keyFrames.Clear();
        }

        public object Evaluate(TimeSpan time, bool forward = true)
        {
            Contract.Requires<InvalidOperationException>(Length > 0, "Animation must contain at least one KeyFrames");
            TKeyFrame start;
            TKeyFrame end;
            if (forward)
            {
                start = keyFrames.Last(kf => kf.Time <= time);
                end = keyFrames.FirstOrDefault(kf => kf.Time > time) ?? keyFrames.First(kf => kf.Time == time);
            }
            else
            {
                start = keyFrames.LastOrDefault(kf => kf.Time > time) ?? keyFrames.First(kf => kf.Time == time);
                end = keyFrames.First(kf => kf.Time <= time);
            }

            return Function(start, end, time);
        }

        #region IResourceProvider

        public void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            TargetProperty = xmlReader.GetAttribute("TargetProperty");
            TargetName = Text.ParseResource(xmlReader.GetAttribute("TargetName"));
            if (!resourceProvider.ContainsResource(TargetName))
                throw new InvalidOperationException(string.Format("No resource '{0}' found", TargetName));

            xmlReader.ReadStartElement();

            while (xmlReader.IsStartElement())
            {
                string type = xmlReader.LocalName;
                var keyFrame = (ISerializableResource)Activator.CreateInstance(Type.GetType(String.Format("Odyssey.Animations.{0}, Odyssey.Common", type)));
                keyFrame.DeserializeXml(resourceProvider, xmlReader);
                AddKeyFrame((TKeyFrame)keyFrame);
            }

            xmlReader.ReadEndElement();
        }

        #endregion IXmlSerializable

        protected static float Map(TimeSpan start, TimeSpan end, TimeSpan value)
        {
            float low = (float)(start.TotalMilliseconds);
            float high = (float)(end.TotalMilliseconds);
            float time = (float)(value.TotalMilliseconds);
            return (time - low) / (high - low);
        }

        #region IEnumerable<TKeyFrame>
        public IEnumerator<TKeyFrame> GetEnumerator()
        {
            return ((IEnumerable<TKeyFrame>)keyFrames).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return keyFrames.GetEnumerator();
        } 
        #endregion
    }
}