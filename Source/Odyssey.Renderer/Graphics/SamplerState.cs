using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odyssey.Engine;
using SharpDX.Mathematics;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    /// <summary>
    /// SamplerState is equivalent to <see cref="SharpDX.Direct3D11.SamplerState"/>.
    /// </summary>
    /// <msdn-id>ff476588</msdn-id>	
    /// <unmanaged>ID3D11SamplerState</unmanaged>	
    /// <unmanaged-short>ID3D11SamplerState</unmanaged-short>	
    public class SamplerState : GraphicsResource
    {
        /// <summary>
        /// Gets the description of this sampler state.
        /// </summary>
        public readonly SamplerStateDescription Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerState" /> class.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description">The description.</param>
        private SamplerState(DirectXDevice device, SamplerStateDescription description)
            : base(device)
        {
            Description = description;
            Initialize(new SharpDX.Direct3D11.SamplerState(device, description));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SamplerState" /> class.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="nativeState">State of the native.</param>
        private SamplerState(DirectXDevice device, SharpDX.Direct3D11.SamplerState nativeState)
            : base(device)
        {
            Description = nativeState.Description;
            Initialize(nativeState);
        }

        public override void Initialize()
        {
            Initialize(Resource);
        }

        /// <summary>	
        /// <p>Create a sampler-state object that encapsulates sampling information for a texture.</p>	
        /// </summary>	
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="samplerState">An existing <see cref="Direct3D11.SamplerState"/> instance.</param>	
        /// <returns>A new <see cref="SamplerState"/> instance</returns>	
        /// <remarks>	
        /// <p>4096 unique sampler state objects can be created on a device at a time.</p><p>If an application attempts to create a sampler-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique sampler state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476518</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateSamplerState([In] const D3D11_SAMPLER_DESC* pSamplerDesc,[Out, Fast] ID3D11SamplerState** ppSamplerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateSamplerState</unmanaged-short>	
        public static SamplerState New(DirectXDevice device, SharpDX.Direct3D11.SamplerState samplerState)
        {
            return new SamplerState(device, samplerState);
        }

        /// <summary>	
        /// <p>Create a sampler-state object that encapsulates sampling information for a texture.</p>	
        /// </summary>	
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description">A sampler state description</param>	
        /// <returns>A new <see cref="SamplerState"/> instance</returns>	
        /// <remarks>	
        /// <p>4096 unique sampler state objects can be created on a device at a time.</p><p>If an application attempts to create a sampler-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique sampler state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476518</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateSamplerState([In] const D3D11_SAMPLER_DESC* pSamplerDesc,[Out, Fast] ID3D11SamplerState** ppSamplerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateSamplerState</unmanaged-short>	
        public static SamplerState New(DirectXDevice device, SamplerStateDescription description)
        {
            return new SamplerState(device, description);
        }

        /// <summary>	
        /// <p>Create a sampler-state object that encapsulates sampling information for a texture.</p>	
        /// </summary>	
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="name">Name of this sampler state.</param>
        /// <param name="description">A sampler state description</param>	
        /// <returns>A new <see cref="SamplerState"/> instance</returns>	
        /// <remarks>	
        /// <p>4096 unique sampler state objects can be created on a device at a time.</p><p>If an application attempts to create a sampler-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique sampler state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476518</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateSamplerState([In] const D3D11_SAMPLER_DESC* pSamplerDesc,[Out, Fast] ID3D11SamplerState** ppSamplerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateSamplerState</unmanaged-short>	
        public static SamplerState New(DirectXDevice device, string name, SamplerStateDescription description)
        {
            return new SamplerState(device, description) { Name = name };
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator SharpDX.Direct3D11.SamplerState(SamplerState from)
        {
            return (SharpDX.Direct3D11.SamplerState)(from == null ? null : from.Resource);
        }

        internal static SamplerState New(DirectXDevice device, string name, Filter filterMode, TextureAddressMode uvwMode)
        {
            var description = SamplerStateDescription.Default();

            // For 9.1, anisotropy cannot be larger then 2
            if (device.Features.Level == FeatureLevel.Level_9_1)
            {
                description.MaximumAnisotropy = 2;
            }

            description.Filter = filterMode;
            description.AddressU = uvwMode;
            description.AddressV = uvwMode;
            description.AddressW = uvwMode;
            return New(device, name, description);
        }
    }
}
