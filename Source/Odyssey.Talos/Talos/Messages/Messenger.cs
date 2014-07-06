using Odyssey.Talos.Maps;
using Odyssey.Talos.Systems;
using System.Linq;

namespace Odyssey.Talos.Messages
{
    public class Messenger
    {
        readonly MessageMap messageMap;

        internal MessageMap MessageMap {get {return messageMap;}}

        public Messenger()
        {
            messageMap = new MessageMap();
        }

        public void Register<TMessage>(ISystem system)
            where TMessage : IMessage
        {
            messageMap.Add<TMessage>(system);
            system.MessageQueue.Associate<TMessage>();
        }

        public void Unregister<TMessage>(ISystem system)
            where TMessage : IMessage
        {
            messageMap.Remove<TMessage>(system);
            system.MessageQueue.RemoveAssociation<TMessage>();
        }

        public void Send<TMessage>(TMessage message)
            where TMessage : IMessage
        {
            var interestedSystems = messageMap.Select<TMessage>();
            foreach (ISystem system in interestedSystems)
                SendTo(message, system);
        }

        public void SendTo<TMessage>(TMessage message, ISystem system)
            where TMessage : IMessage
        {
            if (!message.IsSynchronous)
                system.EnqueueMessage(message);
            else
                system.ReceiveBlockingMessage(message);
            
        }

        /// <summary>
        /// Sends a message to a group of ISystem whose <see cref="Aspect"/> matches 
        /// the one supplied as a parameter
        /// </summary>
        /// <typeparam name="TMessage">The type of the <see cref="IMessage"/>.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="aspect">The aspect.</param>
        public void SendTo<TMessage>(TMessage message, Aspect aspect)
            where TMessage : IMessage
        {
            var interestedSystems = messageMap.Select<TMessage>().Where(s => s.Aspect == aspect);
            foreach (ISystem system in interestedSystems)
                SendTo(message, system);
        }

    }
}
