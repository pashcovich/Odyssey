using Odyssey.Engine;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    public class RasterizerState : GraphicsResource
    {
        private readonly RasterizerStateDescription description;

        public RasterizerStateDescription Description
        {
            get { return description; }
        }

        public override void Initialize()
        {
            Initialize(Resource);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterizerState" /> class.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description">The description.</param>
        private RasterizerState(DirectXDevice device, RasterizerStateDescription description)
            : base(device)
        {
            this.description = description;
            Resource = new SharpDX.Direct3D11.RasterizerState(Device, description);
        }

        /// <summary>	
        /// <p>Create a rasterizer state object that tells the rasterizer stage how to behave.</p>	
        /// </summary>	
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description">A rasterizer state description</param>	
        /// <remarks>	
        /// <p>4096 unique rasterizer state objects can be created on a device at a time.</p><p>If an application attempts to create a rasterizer-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique rasterizer state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476516</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateRasterizerState([In] const D3D11_RASTERIZER_DESC* pRasterizerDesc,[Out, Fast] ID3D11RasterizerState** ppRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateRasterizerState</unmanaged-short>	
        public static RasterizerState New(DirectXDevice device, RasterizerStateDescription description)
        {
            return new RasterizerState(device, description);
        }

        /// <summary>	
        /// <p>Create a rasterizer state object that tells the rasterizer stage how to behave.</p>	
        /// </summary>	
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="name">Name of this depth stencil state.</param>
        /// <param name="description">A rasterizer state description</param>	
        /// <remarks>	
        /// <p>4096 unique rasterizer state objects can be created on a device at a time.</p><p>If an application attempts to create a rasterizer-state interface with the same state as an existing interface, the same interface will be returned and the total number of unique rasterizer state objects will stay the same.</p>	
        /// </remarks>	
        /// <msdn-id>ff476516</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateRasterizerState([In] const D3D11_RASTERIZER_DESC* pRasterizerDesc,[Out, Fast] ID3D11RasterizerState** ppRasterizerState)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateRasterizerState</unmanaged-short>	
        public static RasterizerState New(DirectXDevice device, string name, RasterizerStateDescription description)
        {
            return new RasterizerState(device, description) {Name = name};
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsState to convert from.</param>
        public static implicit operator SharpDX.Direct3D11.RasterizerState(RasterizerState from)
        {
            return (SharpDX.Direct3D11.RasterizerState) (from == null ? null : from.Resource);
        }

        internal static RasterizerState New(DirectXDevice device, string name, CullMode mode)
        {
            var description = RasterizerStateDescription.Default();
            description.CullMode = mode;

            var state = New(device, description);
            state.Name = name;
            return state;
        }

        internal static RasterizerStateDescription Solid
        {
            get
            {
                return new RasterizerStateDescription
                {
                    CullMode = CullMode.Back,
                    IsDepthClipEnabled = true,
                    FillMode = FillMode.Solid,
                    IsAntialiasedLineEnabled = true,
                    IsFrontCounterClockwise = false,
                    IsMultisampleEnabled = true,
                    DepthBias = 100,
                    DepthBiasClamp = 0.0f,
                    SlopeScaledDepthBias = 1.0f,
                };
            }
        }

        internal static RasterizerStateDescription Wireframe
        {
            get
            {
                return
                    new RasterizerStateDescription
                    {
                        CullMode = CullMode.None,
                        IsDepthClipEnabled = true,
                        FillMode = FillMode.Wireframe,
                        IsAntialiasedLineEnabled = true,
                        IsFrontCounterClockwise = false,
                        IsMultisampleEnabled = true
                    };
            }

        }
    }
}
