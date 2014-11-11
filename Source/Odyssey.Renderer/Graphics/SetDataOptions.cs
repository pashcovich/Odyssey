using System;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    /// <summary>
    /// Describes whether existing vertex or index buffer data will be overwritten or discarded during a SetData operation.
    /// </summary>
    [Flags]
    public enum SetDataOptions
    {
        /// <summary>
        /// Portions of existing data in the buffer may be overwritten during this operation.
        /// </summary>
        None,

        /// <summary>
        /// The SetData operation will discard the entire buffer. A pointer to a new memory area is returned so that the direct memory access (DMA) and rendering from the previous area do not stall.
        /// </summary>
        Discard,

        /// <summary>
        /// The SetData operation will not overwrite existing data in the vertex and index buffers. Specifying this option allows the driver to return immediately from a SetData operation and continue rendering.
        /// </summary>
        NoOverwrite
    }

    class SetDataOptionsHelper
    {
        public static MapMode ConvertToMapMode(SetDataOptions options)
        {
            switch (options)
            {
                case SetDataOptions.None:
                    return MapMode.Write;
                case SetDataOptions.Discard:
                    return MapMode.WriteDiscard;
                case SetDataOptions.NoOverwrite:
                    return MapMode.WriteNoOverwrite;
            }
            return MapMode.Write;
        }
    }
}
