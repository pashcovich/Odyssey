using System.ComponentModel;
using System.ServiceModel.Channels;
using Odyssey.Talos.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using SharpDX;
using SharpYaml.Serialization;

namespace Odyssey.Talos.Systems
{
    [DebuggerDisplay("Name = {Name}")]
    public abstract class SystemBase : ISystem
    {
        private static int index;
        private readonly long id;
        private readonly MessageQueue messageQueue;
        private readonly string name;

        public event EventHandler<MessageEventArgs> BlockingMessageReceived;

        [YamlMember(0)]
        public string Name { get { return name; } }
        [YamlMember(1)]
        [DefaultValue(true)]
        public bool IsEnabled { get; set; }
        
        [YamlIgnore] 
        public Aspect Aspect { get; private set; }
        [YamlIgnore]
        public long Id { get { return id; } }
        [YamlIgnore]
        public MessageQueue MessageQueue { get { return messageQueue; } }

        [YamlIgnore]
        public Scene Scene { get; private set; }

        [YamlIgnore]
        protected IServiceRegistry Services { get { return Scene.Services; } }

        [YamlIgnore]
        protected IEnumerable<IEntity> Entities { get { return Scene.SystemMap.SelectAllEntities(this);} }

        protected Messenger Messenger { get { return Scene.Messenger; } }
        protected bool HasEntities { get { return Scene.SystemHasEntities(this); } }

        protected SystemBase(Aspect aspect)
        {
            Aspect = aspect;
            name = GetType().Name;
            id =  1 << index;
            index++;

            messageQueue = new MessageQueue();
            IsEnabled = true;
        }


        protected virtual void OnBlockingMessageReceived(MessageEventArgs args)
        {
            var handler = BlockingMessageReceived;
            if (handler != null)
                handler(this, args);
        }

        public void AssignToScene(Scene scene)
        {
            Scene = scene;
        }

        public void EnqueueMessage<TMessage>(TMessage message)
            where TMessage : Odyssey.Talos.Messages.Message
        {
            messageQueue.Enqueue(message);
        }

        public abstract void Start();

        public abstract void Stop();


        public virtual void Unload()
        { }

        public virtual void Initialize()
        { }


        [Pure]
        public bool Supports(long entityKey)
        {
            return Aspect.Interests(entityKey);
        }

        public void ReceiveBlockingMessage<TMessage>(TMessage message)
            where TMessage : Odyssey.Talos.Messages.Message
        {
            OnBlockingMessageReceived(new MessageEventArgs(message));
        }

        [Pure]
        internal bool IsEntityRegistered(IEntity entity)
        {
            return Scene.SystemMap.IsEntityRegisteredToSystem(entity, this);
        }

    }
}