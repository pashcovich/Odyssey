using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    public partial class Buffer
    {
        /// <summary>
        /// Constant buffer helper methods.
        /// </summary>
        public static class Constant
        {
            /// <summary>
            /// Creates a new constant buffer with <see cref="ResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="size">The size in bytes.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer New(DirectXDevice device, int size)
            {
                return Buffer.New(device, size, BufferFlags.ConstantBuffer, ResourceUsage.Dynamic);
            }

            /// <summary>
            /// Creates a new constant buffer with <see cref="ResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <typeparam name="T">Type of the constant buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer<T> New<T>(DirectXDevice device) where T : struct
            {
                return Buffer.New<T>(device, 1, BufferFlags.ConstantBuffer, ResourceUsage.Dynamic);
            }

            /// <summary>
            /// Creates a new constant buffer with <see cref="ResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <typeparam name="T">Type of the constant buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the constant buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer<T> New<T>(DirectXDevice device, ref T value, ResourceUsage usage = ResourceUsage.Dynamic) where T : struct
            {
                return Buffer.New(device, ref value, BufferFlags.ConstantBuffer, usage);
            }

            /// <summary>
            /// Creates a new constant buffer with <see cref="ResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <typeparam name="T">Type of the constant buffer to get the sizeof from</typeparam>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the constant buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer<T> New<T>(DirectXDevice device, T[] value, ResourceUsage usage = ResourceUsage.Dynamic) where T : struct
            {
                return Buffer.New(device, value, BufferFlags.ConstantBuffer, usage);
            }

            /// <summary>
            /// Creates a new constant buffer with <see cref="ResourceUsage.Dynamic"/> usage.
            /// </summary>
            /// <param name="device">The <see cref="DirectXDevice"/>.</param>
            /// <param name="value">The value to initialize the constant buffer.</param>
            /// <param name="usage">The usage of this resource.</param>
            /// <returns>A constant buffer</returns>
            public static Buffer New(DirectXDevice device, DataPointer value, ResourceUsage usage = ResourceUsage.Dynamic)
            {
                return Buffer.New(device, value, 0, BufferFlags.ConstantBuffer, usage);
            }
        }
    }
}
