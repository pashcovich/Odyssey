#region License

// Copyright © 2013-2014 Avengers UTD - Adalberto L. Simeone
// 
// The Odyssey Engine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License Version 3 as published by
// the Free Software Foundation.
// 
// The Odyssey Engine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details at http://gplv3.fsf.org/

#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using Odyssey.Core;
using Odyssey.Epos.Messages;
using Odyssey.Reflection;
using SharpDX;

#endregion

namespace Odyssey.Epos.Systems
{
    public abstract class SystemBase : ISystem
    {
        protected delegate void MessageHandler();
        private readonly Dictionary<Type, MessageHandler> messageHandlers;
        private static int index;
        private readonly long id;
        private readonly MessageQueue messageQueue;
        private readonly string name;

        protected SystemBase(Selector selector)
        {
            messageHandlers = new Dictionary<Type, MessageHandler>();
            Selector = selector;
            name = GetType().Name;
            id = 1 << index;
            index++;

            messageQueue = new MessageQueue();
            IsEnabled = true;
        }

        public string Name
        {
            get { return name; }
        }

        protected IServiceRegistry Services
        {
            get { return Scene.Services; }
        }

        protected IEnumerable<Entity> Entities
        {
            get { return Scene.SystemMap.SelectAllEntities(this); }
        }

        protected Messenger Messenger
        {
            get { return Scene.Messenger; }
        }

        protected bool HasEntities
        {
            get { return Scene.SystemMap.SystemHasEntities(this); }
        }

        public bool IsEnabled { get; set; }

        public Selector Selector { get; private set; }

        public long Id
        {
            get { return id; }
        }

        public MessageQueue MessageQueue
        {
            get { return messageQueue; }
        }

        public Scene Scene { get; private set; }

        public void AssignToScene(Scene scene)
        {
            Scene = scene;
        }

        public void EnqueueMessage<TMessage>(TMessage message)
            where TMessage : Message
        {
            messageQueue.Enqueue(message);
        }

        protected void Subscribe<TMessage>(MessageHandler handler)
            where TMessage : Message
        {
            Messenger.Register<TMessage>(this);
            var type = typeof (TMessage);
            messageHandlers.Add(type, handler);
        }

        protected void Unsubscribe<TMessage>()
             where TMessage : Message
        {
            Messenger.Unregister<TMessage>(this);
            messageHandlers.Remove(typeof (TMessage));
        }

        private void Message(EntityChangeMessage t ){}

        public abstract void Start();

        public abstract void Stop();


        public virtual void Unload()
        {
        }

        public virtual void Initialize()
        {
        }

        public void RegisterEntity(Entity entity)
        {
            Scene.SystemMap.RegisterEntityToSystem(entity, this);
        }

        public void UnregisterEntity(Entity entity)
        {
            Scene.SystemMap.UnregisterEntityFromSystem(entity, this);
        }

        public bool IsEntityRegistered(Entity entity)
        {
            return Scene.SystemMap.IsEntityRegisteredToSystem(entity, this);
        }

        [Pure]
        public bool Supports(long entityKey)
        {
            return Selector.Interests(entityKey);
        }

        public void ReceiveBlockingMessage<TMessage>(TMessage message)
            where TMessage : Message
        {
            OnBlockingMessageReceived(new MessageEventArgs(message));
        }

        public event EventHandler<MessageEventArgs> BlockingMessageReceived;

        protected virtual void OnBlockingMessageReceived(MessageEventArgs args)
        {
            HandleMessages();
            var handler = BlockingMessageReceived;
            if (handler != null)
                handler(this, args);
        }

        protected virtual void HandleMessages()
        {
            foreach (var kvp in messageHandlers)
            {
                if (MessageQueue.HasItems(kvp.Key))
                    kvp.Value();
            }
        }
    }
}