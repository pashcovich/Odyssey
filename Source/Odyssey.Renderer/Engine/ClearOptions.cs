using System;

namespace Odyssey.Engine
{
    /// <summary>
    /// Specifies the buffer to use when using <see cref="DirectXDevice.Clear(SharpDX.Color4)"/>
    /// </summary>
    [Flags]
    public enum ClearOptions
    {
        /// <summary>
        /// Clears the depth buffer.
        /// </summary>
        DepthBuffer = 2,

        /// <summary>
        /// Clears the stencil buffer.
        /// </summary>
        Stencil = 4,

        /// <summary>
        /// Clears the render target buffer.
        /// </summary>
        Target = 1
    }
}
