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
        readonly Dictionary<string, IAnimationCurve> IAnimationCurves;

        public string Name { get; set; }
        public float Duration { get { return IAnimationCurves.Values.Max(a => a.Duration); } }
        public WrapMode WrapMode { get; set; }

        public IEnumerable<IAnimationCurve> Curves { get { return IAnimationCurves.Values; } }

        public Animation()
        {
            IAnimationCurves = new Dictionary<string, IAnimationCurve>();
        }

        [Pure]
        public bool Contains(IAnimationCurve animationCurve)
        {
            Contract.Requires<ArgumentNullException>(animationCurve != null, "animationCurve");
            return IAnimationCurves.ContainsKey(animationCurve.TargetProperty);
        }

        public void AddCurve(IAnimationCurve animationCurve)
        {
            Contract.Requires<ArgumentException>(!Contains(animationCurve), "curve");
            IAnimationCurves.Add(animationCurve.TargetProperty, animationCurve);
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
                AddCurve((IAnimationCurve)curve);
            }
            
            xmlReader.ReadEndElement();
        }
        #endregion

    }
}
