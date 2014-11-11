using System;
using Odyssey.Collections;
using Odyssey.Core;
using Odyssey.Engine;

namespace Odyssey.Graphics
{
    /// <summary>
    /// Base collection for Graphics device states (BlendState, DepthStencilState, RasterizerState).
    /// </summary>
    /// <typeparam name="T">Type of the state.</typeparam>
    public abstract class StateCollectionBase<T> : ComponentCollection<T>, IDisposable where T : ComponentBase
    {
        /// <summary>
        /// An allocator of state.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="name">The name of the state to create.</param>
        /// <returns>An instance of T or null if not supported.</returns>
        public delegate T StateAllocatorDelegate(DirectXDevice device, string name);

        /// <summary>
        /// Gets the graphics device associated with this collection.
        /// </summary>
        protected readonly DirectXDevice DirectXDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateCollectionBase{T}" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        protected StateCollectionBase(DirectXDevice device)
        {
            DirectXDevice = device;
        }

        /// <summary>
        /// Sets this callback to create a state when a state with a particular name is not found.
        /// </summary>
        public StateAllocatorDelegate StateAllocatorCallback;

        void IDisposable.Dispose()
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                ((IDisposable)Items[i]).Dispose();
            }
            Items.Clear();
        }

        /// <summary>
        /// Registers the specified state.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <remarks>
        /// The name of the state must be defined.
        /// </remarks>
        public void Register(T state)
        {
            Add(state);
        }

        protected override T TryToGetOnNotFound(string name)
        {
            var handler = StateAllocatorCallback;
            if (handler != null) return handler(DirectXDevice, name);
            return default(T);
        }

    }
}
