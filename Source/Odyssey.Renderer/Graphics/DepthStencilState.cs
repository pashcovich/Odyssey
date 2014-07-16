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
    /// DepthStencilState is equivalent to <see cref="SharpDX.Direct3D11.DepthStencilState"/>.
    /// </summary>
    /// <msdn-id>ff476375</msdn-id>	
    /// <unmanaged>ID3D11DepthStencilState</unmanaged>	
    /// <unmanaged-short>ID3D11DepthStencilState</unmanaged-short>	
    public class DepthStencilState : GraphicsResource
    {
        /// <summary>
        /// Gets the description of this blend state.
        /// </summary>
        public readonly DepthStencilStateDescription Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilState" /> class.
        /// </summary>
        /// <param name="device">The device local.</param>
        /// <param name="description">The description.</param>
        private DepthStencilState(DirectXDevice device, DepthStencilStateDescription description)
            : base(device)
        {
            Description = description;
           Resource = new SharpDX.Direct3D11.DepthStencilState(device, Description);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthStencilState" /> class.
        /// </summary>
        /// <param name="device">The device local.</param>
        /// <param name="nativeState">State of the native.</param>
        private DepthStencilState(DirectXDevice device, SharpDX.Direct3D11.DepthStencilState nativeState)
            : base(device)
        {
            Description = nativeState.Description;
            Resource = nativeState;
        }

        public override void Initialize()
        {
            Initialize(Resource);
        }

        /// <summary>	
        /// Create a depth-stencil state object that encapsulates depth-stencil test information for the output-merger stage.
        /// </summary>	
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="depthStencilState">An existing <see cref="Direct3D11.DepthStencilState"/> instance.</param>	
        /// <returns>A new instance of <see cref="DepthStencilState"/></returns>	
        /// <remarks>	
        /// <p>4096 unique depth-stencil state objects can be created on a device at a time.</p><p>If an application attempts to create a depth-stencil-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique depth-stencil state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476506</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateDepthStencilState([In] const D3D11_DEPTH_STENCIL_DESC* pDepthStencilDesc,[Out, Fast] ID3D11DepthStencilState** ppDepthStencilState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateDepthStencilState</unmanaged-short>	
        public static DepthStencilState New(DirectXDevice device, SharpDX.Direct3D11.DepthStencilState depthStencilState)
        {
            return new DepthStencilState(device, depthStencilState);
        }

        /// <summary>	
        /// Create a depth-stencil state object that encapsulates depth-stencil test information for the output-merger stage.
        /// </summary>	
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description">A depth-stencil state description</param>	
        /// <returns>A new instance of <see cref="DepthStencilState"/></returns>	
        /// <remarks>	
        /// <p>4096 unique depth-stencil state objects can be created on a device at a time.</p><p>If an application attempts to create a depth-stencil-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique depth-stencil state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476506</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateDepthStencilState([In] const D3D11_DEPTH_STENCIL_DESC* pDepthStencilDesc,[Out, Fast] ID3D11DepthStencilState** ppDepthStencilState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateDepthStencilState</unmanaged-short>	
        public static DepthStencilState New(DirectXDevice device, DepthStencilStateDescription description)
        {
            return new DepthStencilState(device, description);
        }

        /// <summary>	
        /// Create a depth-stencil state object that encapsulates depth-stencil test information for the output-merger stage.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="name">Name of this depth stencil state.</param>
        /// <param name="description">A depth-stencil state description</param>	
        /// <returns>A new instance of <see cref="DepthStencilState"/></returns>	
        /// <remarks>	
        /// <p>4096 unique depth-stencil state objects can be created on a device at a time.</p><p>If an application attempts to create a depth-stencil-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique depth-stencil state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476506</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateDepthStencilState([In] const D3D11_DEPTH_STENCIL_DESC* pDepthStencilDesc,[Out, Fast] ID3D11DepthStencilState** ppDepthStencilState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateDepthStencilState</unmanaged-short>	
        public static DepthStencilState New(DirectXDevice device, string name, DepthStencilStateDescription description)
        {
            return new DepthStencilState(device, description) { Name = name };
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator SharpDX.Direct3D11.DepthStencilState(DepthStencilState from)
        {
            return (SharpDX.Direct3D11.DepthStencilState)(from == null ? null : from.Resource);
        }

        internal static DepthStencilState New(DirectXDevice device, string name, bool depthEnable, bool depthWriteEnable)
        {
            var description = DepthStencilStateDescription.Default();
            description.IsDepthEnabled = depthEnable;
            description.DepthWriteMask = depthWriteEnable ? DepthWriteMask.All : DepthWriteMask.Zero;

            var state = New(device, description);
            state.Name = name;
            return state;
        }

        internal static DepthStencilStateDescription Default
        {
            get
            {
                var description = DepthStencilStateDescription.Default();
                description.IsDepthEnabled = true;
                description.DepthWriteMask = DepthWriteMask.All;
                return description;
            }
        }

    }
}
