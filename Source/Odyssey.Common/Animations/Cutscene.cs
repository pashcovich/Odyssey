#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Odyssey.Content;
using Odyssey.Core;
using Odyssey.Engine;
using Odyssey.Organization.Commands;
using Odyssey.Serialization;

#endregion

namespace Odyssey.Animations
{
    [ContentReader(typeof (CutsceneContentReader))]
    public class Cutscene : AnimationController, ISerializableResource
    {
        private readonly CommandManager commandManager;
        private readonly List<Command> executedCommands;
        private readonly IServiceRegistry services;
        private float elapsedTime;
        private AnimationStatus status;

        public Cutscene(IServiceRegistry services)
        {
            this.services = services;
            commandManager = new CommandManager();
            executedCommands = new List<Command>();
        }

        public AnimationStatus Status
        {
            get { return status; }
        }

        public override void Update(ITimeService time)
        {
            if (status == AnimationStatus.Stopped)
                return;
            elapsedTime += time.FrameTime;
            var cAnimation = commandManager.OfType<PlayAnimationCommand>().LastOrDefault(c => elapsedTime > c.Time);
            if (cAnimation != null && !executedCommands.Contains(cAnimation))
            {
                cAnimation.Execute();
                executedCommands.Add(cAnimation);
            }
            base.Update(time);
        }

        public override void Play()
        {
            status = AnimationStatus.Playing;
            base.Play();
        }

        #region ISerializableResource

        public void SerializeXml(IResourceProvider resourceProvider, XmlWriter xmlWriter)
        {
            throw new NotImplementedException();
        }

        public void DeserializeXml(IResourceProvider resourceProvider, XmlReader xmlReader)
        {
            string sCutscene = typeof (Cutscene).Name;
            string sAnimation = typeof (Animation).Name;
            const string sCommands = "Commands";
            Target = resourceProvider;

            // Deserialize animations
            xmlReader.ReadStartElement(sCutscene);

            while (xmlReader.IsStartElement(sAnimation))
            {
                var animation = new Animation();
                animation.DeserializeXml(resourceProvider, xmlReader);
                AddAnimation(animation);
            }
            if (xmlReader.IsStartElement(sCommands))
            {
                xmlReader.ReadStartElement();
                while (xmlReader.IsStartElement())
                {
                    var animationCommand = new PlayAnimationCommand(services);
                    animationCommand.DeserializeXml(resourceProvider, xmlReader);
                    commandManager.AddLast(animationCommand);
                }
            }
            xmlReader.ReadEndElement();
        }

        #endregion
    }
}