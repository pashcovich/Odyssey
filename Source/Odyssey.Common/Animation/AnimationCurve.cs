using Odyssey.Engine;
using Odyssey.Utilities.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Odyssey.Animation
{
    internal delegate void UpdateMethod(object obj, object value);

    public class AnimationCurve : IAnimationCurve, IXmlSerializable
    {
        public delegate object CurveFunction(IKeyFrame start, IKeyFrame end, TimeSpan time);

        private List<IKeyFrame> keyFrames;

        private string targetName;
        private string targetPropertyName;

        private PropertyInfo targetProperty;
        private FieldInfo field;
        private TimeSpan elapsedTime;
        private object target;
        private UpdateMethod updateMethod;

        public int Length { get { return keyFrames.Count; } }

        /// <inheritdoc/>
        public float Duration { get { return (float)keyFrames.Max().Time.TotalSeconds; } }

        public string Name { get; private set; }

        public CurveFunction Function { get; set; }

        public bool IsPlaying { get; private set; }

        public AnimationCurve()
        {
            keyFrames = new List<IKeyFrame>();
        }

        public AnimationCurve(string name, Type type, string propertyName)
            : this()
        {
            Name = name;
            SetTargetProperty(type, propertyName);
        }

        internal void SetTargetProperty(Type type, string propertyName)
        {
            var member = ReflectionHelper.FindMemberPath(type, propertyName, out targetProperty);
            if ((member as PropertyInfo) != null)
                updateMethod = UpdateProperty;
            else
            {
                field = (FieldInfo)member;
                updateMethod = UpdateField;
            }
        }

        public void AddKeyFrame(IKeyFrame keyFrame)
        {
            Contract.Requires<ArgumentNullException>(keyFrame != null, "keyFrame");
            keyFrames.Add(keyFrame);
            keyFrames.Sort();
        }

        public void Clear()
        {
            keyFrames.Clear();
        }

        public object Evaluate(TimeSpan time)
        {
            Contract.Requires<InvalidOperationException>(Length > 1, "Animation must contain at least two KeyFrames");
            var start = keyFrames.Last(kf => kf.Time <= time);
            var end = keyFrames.First(kf => kf.Time > time) ?? keyFrames.First(kf => kf.Time == time);

            return Function(start, end, time);
        }

        public void Start()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            elapsedTime = default(TimeSpan);
        }

        public void Update(ITimeService time, object obj)
        {
            var value = Evaluate(elapsedTime);
            updateMethod(obj, value);
            elapsedTime += time.ElapsedApplicationTime;
            if (elapsedTime.TotalSeconds > Duration)
                Stop();
        }

        private void UpdateField(object obj, object value)
        {
            if (target == null)
                target = targetProperty.GetValue(obj);
            field.SetValue(target, value);
            targetProperty.SetValue(obj, target);
        }

        private void UpdateProperty(object obj, object value)
        {
            targetProperty.SetValue(obj, value);
        }

        #region IXmlSerializable

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        {
            Name = reader.GetAttribute("Name");
            targetName = reader.GetAttribute("TargetName");
            targetPropertyName = reader.GetAttribute("TargetProperty");

            reader.ReadStartElement();

            while (reader.IsStartElement())
            {
                string type = reader.LocalName;
                var keyFrame = Activator.CreateInstance(Type.GetType("Odyssey.Animation." + type));
                ((IXmlSerializable)keyFrame).ReadXml(reader);
            }
        }

        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion IXmlSerializable
    }
}