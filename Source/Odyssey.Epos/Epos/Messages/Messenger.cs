using Odyssey.Epos.Components;
using Odyssey.Epos.Maps;
using Odyssey.Epos.Systems;
using System.Linq;

namespace Odyssey.Epos.Messages
{
    public class Messenger
    {
        readonly MessageMap messageMap;

        public Messenger()
        {
            messageMap = new MessageMap();
        }

        public void Register<TMessage>(SystemBase system)
            where TMessage : Message
        {
            messageMap.Add<TMessage>(system);
            system.MessageQueue.Associate<TMessage>();
        }

        public void Unregister<TMessage>(SystemBase system)
            where TMessage : Message
        {
            messageMap.Remove<TMessage>(system);
            system.MessageQueue.RemoveAssociation<TMessage>();
        }

        public void Send<TMessage>(TMessage message, bool isBlocking = false)
            where TMessage : Message
        {
            var interestedSystems = messageMap.Select<TMessage>();
            foreach (SystemBase system in interestedSystems)
                SendToSystem(message, system, isBlocking);
        }

        internal void SendToSystem(Message message, SystemBase system, bool isBlocking)
        {
            system.EnqueueMessage(message);
            if (isBlocking)
                system.ReceiveBlockingMessage(message);
        }

        public void SendToSystem<TSystem, TMessage>(TMessage message, bool isBlocking = false)
            where TSystem : SystemBase
            where TMessage : Message
        {
            var system = messageMap.Select<TMessage>().OfType<TSystem>().First();
            SendToSystem(message, system, isBlocking);
        }

        public void SendToEntity<TComponent>(Message message, Entity target)
            where TComponent : Component
        {
            target.GetComponent<TComponent>().ReceiveMessage(message);
        }

        /// <summary>
        /// Sends a message to a group of ISystem whose <see cref="Selector"/> matches 
        /// the one supplied as a parameter
        /// </summary>
        /// <typeparam name="TMessage">The type of the <see cref="Message"/>.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="selector">The aspect.</param>
        public void SendToSystems<TMessage>(TMessage message, Selector selector)
            where TMessage : Message
        {
            var interestedSystems = messageMap.Select<TMessage>().Where(s => s.Selector == selector);
            foreach (SystemBase system in interestedSystems)
                SendToSystem(message, system, false);
        }

    }
}
