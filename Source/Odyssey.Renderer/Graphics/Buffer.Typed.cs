using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using SharpDX;
using SharpDX.Mathematics;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Typed buffer helper methods.
        /// </summary>
        /// <remarks>
        /// Example in HLSL: Buffer&lt;float4&gt;.
        /// </remarks>
        public static class Typed
        {
            /// <summary>
            /// Creates a new Typed buffer <see cref="ResourceUsage.Default" /> usage.
            /// </summary>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="count">The number of data with the following viewFormat.</param>
            /// <param name="viewFormat">The view format of the buffer.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <param name="usage">The usage.</param>
            /// <returns>A Typed buffer</returns>
            public static Buffer New(DirectXDevice device, int count, PixelFormat viewFormat,
                bool isUnorderedAccess = false, ResourceUsage usage = ResourceUsage.Default)
            {
                return Buffer.New(device, count*viewFormat.SizeInBytes,
                    BufferFlags.ShaderResource | (isUnorderedAccess ? BufferFlags.UnorderedAccess : BufferFlags.None),
                    viewFormat, usage);
            }

            /// <summary>
            /// Creates a new Typed buffer <see cref="ResourceUsage.Default" /> usage.
            /// </summary>
            /// <typeparam name="T">Type of the Typed buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the Typed buffer.</param>
            /// <param name="viewFormat">The view format of the buffer.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Typed buffer</returns>
            public static Buffer<T> New<T>(DirectXDevice device, T[] value, PixelFormat viewFormat,
                bool isUnorderedAccess = false, ResourceUsage usage = ResourceUsage.Default) where T : struct
            {
                return Buffer.New(device, value,
                    BufferFlags.ShaderResource | (isUnorderedAccess ? BufferFlags.UnorderedAccess : BufferFlags.None),
                    viewFormat, usage);
            }

            /// <summary>
            /// Creates a new Typed buffer <see cref="ResourceUsage.Default" /> usage.
            /// </summary>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the Typed buffer.</param>
            /// <param name="viewFormat">The view format of the buffer.</param>
            /// <param name="isUnorderedAccess">if set to <c>true</c> this buffer supports unordered access (RW in HLSL).</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A Typed buffer</returns>
            public static Buffer New(DirectXDevice device, DataPointer value, PixelFormat viewFormat,
                bool isUnorderedAccess = false, ResourceUsage usage = ResourceUsage.Default)
            {
                return Buffer.New(device, value, 0,
                    BufferFlags.ShaderResource | (isUnorderedAccess ? BufferFlags.UnorderedAccess : BufferFlags.None),
                    viewFormat, usage);
            }
        }
    }
}
