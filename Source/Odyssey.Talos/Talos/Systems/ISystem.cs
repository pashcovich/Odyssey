using Odyssey.Talos.Messages;

namespace Odyssey.Talos.Systems
{
    public interface ISystem
    {
        /// <summary>
        /// Returns the composed ID resulting from the set of <see cref="IComponent"/>
        /// this ISystem is associated to.
        /// </summary>
        Selector Selector { get; }
        long Id { get; }
        bool IsEnabled { get; }

        MessageQueue MessageQueue { get; }

        Scene Scene { get; }

        void AssignToScene(Scene scene);

        void EnqueueMessage<TMessage>(TMessage message)
            where TMessage : Message;

        void Initialize();
        void Start();

        void ReceiveBlockingMessage<TMessage>(TMessage message)
            where TMessage : Message;

        void Stop();

        /// <summary>
        /// Checks whether the ISystem supports the <see cref="IEntity"/> identified by its key./>
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>$(True) if the <paramref name="key"/> is supported; $(False) otherwise.</returns>
        bool Supports(long key);
        
        void Unload();
    }
}