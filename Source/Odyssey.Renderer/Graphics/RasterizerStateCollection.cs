using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using SharpDX.Direct3D11;
using SharpDX.Win32;

namespace Odyssey.Graphics
{
    // <summary>
    /// Rasterizer state collection.
    /// </summary>
    public sealed class RasterizerStateCollection : StateCollectionBase<RasterizerState>
    {
        /// <summary>
        /// Built-in rasterizer state object with settings for wireframe rendering.
        /// </summary>
        public readonly RasterizerState WireFrame;

        /// <summary>
        /// Built-in rasterizer state object with settings for wireframe rendering.
        /// </summary>
        public readonly RasterizerState WireFrameCullNone;

        /// <summary>
        /// Built-in rasterizer state object with settings for culling primitives with clockwise winding order (front facing).
        /// </summary>
        public readonly RasterizerState CullFront;

        /// <summary>
        /// Built-in rasterizer state object with settings for culling primitives with counter-clockwise winding order (back facing).
        /// </summary>
        public readonly RasterizerState CullBack;

        /// <summary>
        /// Built-in rasterizer state object with settings for not culling any primitives.
        /// </summary>
        public readonly RasterizerState CullNone;

        /// <summary>
        /// Built-in default rasterizer state object is back facing (see <see cref="CullBack"/>).
        /// </summary>
        public readonly RasterizerState Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerStateCollection" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        internal RasterizerStateCollection(DirectXDevice device)
            : base(device)
        {
            CullFront = Add(RasterizerState.New(device, "CullFront", CullMode.Front));
            CullBack = Add(RasterizerState.New(device, "CullBack", CullMode.Back));
            CullNone = Add(RasterizerState.New(device, "CullNone", CullMode.None));

            var wireFrameDesk = CullBack.Description;
            wireFrameDesk.FillMode = FillMode.Wireframe;
            WireFrame = Add(RasterizerState.New(device, "WireFrame", wireFrameDesk));

            wireFrameDesk.CullMode = CullMode.None;
            WireFrameCullNone = Add(RasterizerState.New(device, "WireFrameCullNone", wireFrameDesk));

            Default = CullBack;

            foreach (var state in Items)
                state.Initialize();
        }
    }
}
