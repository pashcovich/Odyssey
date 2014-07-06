using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    /// <summary>
    /// Base class extension for <see cref="SharpDX.Direct3D11.Buffer"/>.
    /// </summary>
    /// <msdn-id>ff476351</msdn-id>	
    /// <unmanaged>ID3D11Buffer</unmanaged>	
    /// <unmanaged-short>ID3D11Buffer</unmanaged-short>	
    public abstract class BufferBase : GraphicsResource
    {
        protected internal ShaderResourceView ShaderResourceView;

        protected internal UnorderedAccessView UnorderedAccessView;

        /// <summary>
        /// Gets the description of this buffer.
        /// </summary>
        public readonly BufferDescription Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferBase" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="description">The description.</param>
        protected BufferBase(DirectXDevice deviceLocal, BufferDescription description) : base(deviceLocal)
        {
            Description = description;
            Resource = new SharpDX.Direct3D11.Buffer(deviceLocal, description);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferBase" /> class.
        /// </summary>
        /// <param name="deviceLocal">The device local.</param>
        /// <param name="nativeBuffer">The native buffer.</param>
        protected BufferBase(DirectXDevice deviceLocal, SharpDX.Direct3D11.Buffer nativeBuffer) : base(deviceLocal)
        {
            Description = nativeBuffer.Description;
            Resource = nativeBuffer;
        }

        protected override void Initialize(DeviceChild resource)
        {
            base.Initialize(resource);
            this.InitializeViews();
        }

        /// <summary>
        /// Initializes the views.
        /// </summary>
        protected abstract void InitializeViews();

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns>A new instance of this buffer as a staging resource</returns>
        public abstract BufferBase ToStaging();

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <typeparam name="T">Type of the staging buffer.</typeparam>
        /// <returns>A new instance of this buffer as a staging resource</returns>
        public T ToStaging<T>() where T : BufferBase
        {
            return (T)ToStaging();
        }

        /// <summary>
        /// Creates a <see cref="BufferDescription"/>.
        /// </summary>
        /// <param name="sizeInBytes">The size in bytes.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="isReadWrite">if set to <c>true</c> [is read write].</param>
        /// <param name="structureByteStride">The structure byte stride.</param>
        /// <param name="usage">The usage.</param>
        /// <param name="optionFlags">The option flags.</param>
        /// <returns>An instance of <see cref="BufferDescription"/></returns>
        protected static BufferDescription NewDescription(int sizeInBytes, BindFlags flags, bool isReadWrite = false, int structureByteStride = 0, ResourceUsage usage = ResourceUsage.Default, ResourceOptionFlags optionFlags = ResourceOptionFlags.None)
        {
            var desc = new BufferDescription()
            {
                SizeInBytes = sizeInBytes,
                StructureByteStride = structureByteStride,
                CpuAccessFlags = GetCpuAccessFlagsFromUsage(usage),
                BindFlags = flags,
                OptionFlags = optionFlags,
                Usage = usage,
            };

            if (isReadWrite)
            {
                desc.BindFlags |= BindFlags.UnorderedAccess;
            }
            return desc;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator Resource(BufferBase from)
        {
            return (Resource) @from.Resource;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator SharpDX.Direct3D11.Buffer(BufferBase from)
        {
            return (SharpDX.Direct3D11.Buffer)from.Resource;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator ShaderResourceView(BufferBase from)
        {
            return from.ShaderResourceView;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Buffer"/>
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>The result of the operator.</returns>
        public static implicit operator UnorderedAccessView(BufferBase from)
        {
            return from.UnorderedAccessView;
        }
    }
}
