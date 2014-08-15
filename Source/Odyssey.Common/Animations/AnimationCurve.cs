using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using Odyssey.Content;
using Odyssey.Engine;
using Odyssey.Geometry;
using Odyssey.Graphics;
using Odyssey.Serialization;
using Odyssey.Utilities.Logging;
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
        public delegate object CurveFunction(TKeyFrame start, TKeyFrame end, float time);

        private readonly List<TKeyFrame> keyFrames;
        private TimeSpan elapsedTime;
        private object target;

        internal Animation Animation { get; set; }

        public int Length { get { return keyFrames.Count; } }

        /// <inheritdoc/>
        public float Duration { get { return keyFrames.Max(kf => kf.Time); }}
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

        public object Evaluate(float time)
        {
            Contract.Requires<InvalidOperationException>(Length > 0, "Animation must contain at least one KeyFrames");
            TKeyFrame start = keyFrames.First();
            TKeyFrame end = keyFrames.Last();
            if (time <= start.Time)
                return start.Value;
            else if (time > end.Time)
                return end.Value;

            start = keyFrames.FirstOrDefault(kf => kf.Time < time) ?? start;
            end = keyFrames.First(kf => kf.Time > start.Time);
            object result = Function(start, end, time);

            return result;
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

        protected static float Map(float start, float end, float time)
        {
            float denominator = end - start;
            if (MathHelper.ScalarNearEqual(denominator, 0))
                return 0;

            float result = (time - start) / denominator;
            return MathHelper.Clamp(result, 0, 1);
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