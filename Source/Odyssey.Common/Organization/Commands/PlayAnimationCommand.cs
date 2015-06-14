#region Using Directives

using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Xml;
using Odyssey.Animations;
using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Graphics.Organization;
using Odyssey.Serialization;

#endregion

namespace Odyssey.Organization.Commands
{
    public class PlayAnimationCommand : Command, ISerializableResource
    {
        private string animationName;
        private IAnimatable target;
        private float time;

        public PlayAnimationCommand(IServiceRegistry services)
            : base(services, CommandType.PlayAnimation)
        {
        }

        public PlayAnimationCommand(IServiceRegistry services, AnimationController target, string animationName, float startTime)
            : this(services)
        {
            Contract.Requires<ArgumentNullException>(target != null, "target");
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(animationName), "animationName");
            this.target = target;
            this.animationName = animationName;
            time = startTime;
        }

        public IAnimatable Target
        {
            get { return target; }
        }

        public string AnimationName
        {
            get { return animationName; }
        }

        public float Time
        {
            get { return time; }
        }

        public void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            string targetName = xmlReader.GetAttribute("TargetName");
            target = resourceProvider.GetResource<IAnimatable>(targetName);
            animationName = xmlReader.GetAttribute("AnimationName");
            time = float.Parse(xmlReader.GetAttribute("Time"), CultureInfo.InvariantCulture);
            xmlReader.ReadStartElement();
        }

        public override void Initialize()
        {
            IsInited = true;
        }

        public override void Execute()
        {
            if (string.IsNullOrEmpty(animationName))
                target.Play();
            else
                target.Play(animationName);
        }
    }
}