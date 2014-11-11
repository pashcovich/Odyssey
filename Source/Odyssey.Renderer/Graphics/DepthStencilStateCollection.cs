using Odyssey.Engine;

namespace Odyssey.Graphics
{
    /// <summary>
    /// Depth-stencil state collection.
    /// </summary>
    public sealed class DepthStencilStateCollection : StateCollectionBase<DepthStencilState>
    {
        /// <summary>
        /// A built-in state object with default settings for using a depth stencil buffer.
        /// </summary>
        public readonly DepthStencilState Enabled;

        /// <summary>
        /// A built-in state object with settings for enabling a read-only depth stencil buffer.
        /// </summary>
        public readonly DepthStencilState DepthRead;

        /// <summary>
        /// A built-in state object with settings for not using a depth stencil buffer.
        /// </summary>
        public readonly DepthStencilState None;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilStateCollection" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal DepthStencilStateCollection(DirectXDevice device)
            : base(device)
        {
            Enabled = Add(DepthStencilState.New(device, "Enabled", true, true));
            DepthRead = Add(DepthStencilState.New(device, "DepthRead", true, false));
            None = Add(DepthStencilState.New(device, "None", false, false));

            foreach (var state in Items)
                state.Initialize();
        }
    }
}
