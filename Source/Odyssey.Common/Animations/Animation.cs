using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using Odyssey.Graphics;
using Odyssey.Serialization;

namespace Odyssey.Animations
{
    public class Animation : ISerializableResource, IResource
    {
        readonly Dictionary<string, IAnimationCurve> animationCurves;

        public string Name { get; set; }
        public float Duration { get { return animationCurves.Values.Max(a => a.Duration); } }
        public WrapMode WrapMode { get; set; }
        public float Speed { get; set; }

        public IEnumerable<IAnimationCurve> Curves { get { return animationCurves.Values; } }

        public Animation()
        {
            animationCurves = new Dictionary<string, IAnimationCurve>();
            Speed = 1.0f;
        }

        [Pure]
        public bool Contains(IAnimationCurve animationCurve)
        {
            Contract.Requires<ArgumentNullException>(animationCurve != null, "animationCurve");
            return animationCurves.ContainsKey(animationCurve.TargetProperty);
        }

        public void AddCurve(IAnimationCurve animationCurve)
        {
            Contract.Requires<ArgumentException>(!Contains(animationCurve), "animationCurve");
            animationCurves.Add(animationCurve.TargetProperty, animationCurve);
        }

        public void RemoveCurve(string key)
        {
            animationCurves.Remove(key);
        }

        #region ISerializableResource
        public void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            Name = xmlReader.GetAttribute("Name");
            xmlReader.ReadStartElement();
            string animationType = string.Format("Odyssey.Animations.{0}", xmlReader.LocalName);

            while (xmlReader.IsStartElement())
            {
                ISerializableResource curve;
                try
                {
                    curve = (ISerializableResource) Activator.CreateInstance(Type.GetType(animationType));
                }
                catch (ArgumentNullException)
                {
                    throw new InvalidOperationException(string.Format("Animation type `{0}` is not valid", animationType));
                }
                curve.DeserializeXml(resourceProvider, xmlReader);
                var animationCurve = (IAnimationCurve)curve;
                animationCurve.Name = string.Format("{0}Curve{1:D2}", Name, animationCurves.Count-1);
                AddCurve(animationCurve);
            }
            
            xmlReader.ReadEndElement();
        }
        #endregion

    }
}
