using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Vertex buffer helper methods.
        /// </summary>
        public static class Vertex
        {
            /// <summary>
            /// Creates a new Vertex buffer with <see cref="ResourceUsage.Default"/> usage by default.
            /// </summary>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="size">The size in bytes.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Vertex buffer</returns>
            public static Buffer New(DirectXDevice device, int size, ResourceUsage usage = ResourceUsage.Default)
            {
                return Buffer.New(device, size, BufferFlags.VertexBuffer, usage);
            }

            /// <summary>
            /// Creates a new Vertex buffer with <see cref="ResourceUsage.Default"/> usage by default.
            /// </summary>
            /// <typeparam name="T">Type of the Vertex buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="vertexBufferCount">Number of vertex in this buffer with the sizeof(T).</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Vertex buffer</returns>
            public static Buffer<T> New<T>(DirectXDevice device, int vertexBufferCount,
                ResourceUsage usage = ResourceUsage.Default) where T : struct
            {
                return Buffer.New<T>(device, vertexBufferCount, BufferFlags.VertexBuffer, usage);
            }

            /// <summary>
            /// Creates a new Vertex buffer with <see cref="ResourceUsage.Immutable"/> usage by default.
            /// </summary>
            /// <typeparam name="T">Type of the Vertex buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the Vertex buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Vertex buffer</returns>
            public static Buffer<T> New<T>(DirectXDevice device, T[] value,
                ResourceUsage usage = ResourceUsage.Immutable) where T : struct
            {
                return Buffer.New(device, value, BufferFlags.VertexBuffer, usage);
            }

            /// <summary>
            /// Creates a new Vertex buffer with <see cref="ResourceUsage.Immutable"/> usage by default.
            /// </summary>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the Vertex buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Vertex buffer</returns>
            public static Buffer New(DirectXDevice device, DataPointer value,
                ResourceUsage usage = ResourceUsage.Immutable)
            {
                return Buffer.New(device, value, 0, BufferFlags.VertexBuffer, usage);
            }
        }
    }
}
