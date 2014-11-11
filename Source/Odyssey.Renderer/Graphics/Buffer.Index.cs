using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using SharpDX;
using SharpDX.Mathematics;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Odyssey.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Index buffer helper methods.
        /// </summary>
        public static class Index
        {
            /// <summary>
            /// Creates a new index buffer with <see cref="ResourceUsage.Default"/> usage by default.
            /// </summary>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="size">The size in bytes.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A index buffer</returns>
            public static Buffer New(DirectXDevice device, int size, ResourceUsage usage = ResourceUsage.Default)
            {
                return Buffer.New(device, size, BufferFlags.IndexBuffer, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="ResourceUsage.Default"/> usage by default.
            /// </summary>
            /// <typeparam name="T">Type of the index buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="indexCount">Number of indices.</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A index buffer</returns>
            public static Buffer<T> New<T>(DirectXDevice device, int indexCount,
                ResourceUsage usage = ResourceUsage.Default) where T : struct
            {
                return Buffer.New<T>(device, indexCount, BufferFlags.IndexBuffer, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="ResourceUsage.Immutable"/> usage by default.
            /// </summary>
            /// <typeparam name="T">Type of the index buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the index buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A index buffer</returns>
            public static Buffer<T> New<T>(DirectXDevice device, ref T value,
                ResourceUsage usage = ResourceUsage.Immutable) where T : struct
            {
                return Buffer.New(device, ref value, BufferFlags.IndexBuffer, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="ResourceUsage.Immutable"/> usage by default.
            /// </summary>
            /// <typeparam name="T">Type of the index buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the index buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A index buffer</returns>
            public static Buffer<T> New<T>(DirectXDevice device, T[] value,
                ResourceUsage usage = ResourceUsage.Immutable) where T : struct
            {
                return Buffer.New(device, value, BufferFlags.IndexBuffer, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="ResourceUsage.Immutable"/> usage by default.
            /// </summary>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the index buffer.</param>
            /// <param name="is32BitIndex">Set to true if the buffer is using a 32 bit index or false for 16 bit index.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A index buffer</returns>
            public static Buffer New(DirectXDevice device, byte[] value, bool is32BitIndex,
                ResourceUsage usage = ResourceUsage.Immutable)
            {
                return Buffer.New(device, value, is32BitIndex ? 4 : 2, BufferFlags.IndexBuffer, Format.Unknown, usage);
            }

            /// <summary>
            /// Creates a new index buffer with <see cref="ResourceUsage.Immutable"/> usage by default.
            /// </summary>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the index buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A index buffer</returns>
            public static Buffer New(DirectXDevice device, DataPointer value,
                ResourceUsage usage = ResourceUsage.Immutable)
            {
                return Buffer.New(device, value, 0, BufferFlags.IndexBuffer, usage);
            }
        }
    }
}
