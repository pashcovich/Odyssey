using System;
using Odyssey.Engine;
using SharpDX.Direct3D11;

namespace Odyssey.Graphics
{
    /// <summary>
    /// A RenderTarget2D front end to <see cref="SharpDX.Direct3D11.Texture2D"/>.
    /// </summary>
    /// <remarks>
    /// This class instantiates a <see cref="Texture2D"/> with the binding flags <see cref="BindFlags.RenderTarget"/>.
    /// This class is also castable to <see cref="SharpDX.Direct3D11.RenderTargetView"/>.
    /// </remarks>
    public class RenderTarget2D : Texture2DBase
    {
        private readonly bool pureRenderTarget;
        private readonly RenderTargetView customRenderTargetView;

        internal RenderTarget2D(DirectXDevice device, Texture2DDescription description2D)
            : base(device, description2D)
        {
            Initialize(Resource);
        }

        internal RenderTarget2D(DirectXDevice device, SharpDX.Direct3D11.Texture2D texture, RenderTargetView renderTargetView = null, bool pureRenderTarget = false)
            : base(device, texture)
        {
            this.pureRenderTarget = pureRenderTarget;
            customRenderTargetView = renderTargetView;
            Initialize(Resource);
        }



        /// <summary>
        /// RenderTargetView casting operator.
        /// </summary>
        /// <param name="from">Source for the.</param>
        public static implicit operator RenderTargetView(RenderTarget2D from)
        {
            return from == null ? null : from.renderTargetViews != null ? from.renderTargetViews[0] : null;
        }

        protected override void InitializeViews()
        {
            if ((Description.BindFlags & BindFlags.RenderTarget) != 0)
            {
                renderTargetViews = new TextureView[GetViewCount()];
            }

            if (pureRenderTarget)
            {
                renderTargetViews[0] = new TextureView(this, customRenderTargetView);
            }
            else
            {
                // Perform default initialization
                base.InitializeViews();

                if ((Description.BindFlags & BindFlags.RenderTarget) != 0)
                {
                    GetRenderTargetView(ViewType.Full, 0, 0);
                }
            }
        }

        internal override TextureView GetRenderTargetView(ViewType viewType, int arrayOrDepthSlice, int mipIndex)
        {
            if ((Description.BindFlags & BindFlags.RenderTarget) == 0)
                return null;

            if (viewType == ViewType.MipBand)
                throw new NotSupportedException("ViewSlice.MipBand is not supported for render targets");

            int arrayCount;
            int mipCount;
            GetViewSliceBounds(viewType, ref arrayOrDepthSlice, ref mipIndex, out arrayCount, out mipCount);

            var rtvIndex = GetViewIndex(viewType, arrayOrDepthSlice, mipIndex);

            lock (renderTargetViews)
            {
                var rtv = renderTargetViews[rtvIndex];

                // Creates the shader resource view
                if (rtv == null)
                {
                    // Create the render target view
                    var rtvDescription = new RenderTargetViewDescription { Format = Description.Format };

                    if (Description.ArraySize > 1)
                    {
                        rtvDescription.Dimension = Description.SampleDescription.Count > 1 ? RenderTargetViewDimension.Texture2DMultisampledArray : RenderTargetViewDimension.Texture2DArray;
                        if (Description.SampleDescription.Count > 1)
                        {
                            rtvDescription.Texture2DMSArray.ArraySize = arrayCount;
                            rtvDescription.Texture2DMSArray.FirstArraySlice = arrayOrDepthSlice;
                        }
                        else
                        {
                            rtvDescription.Texture2DArray.ArraySize = arrayCount;
                            rtvDescription.Texture2DArray.FirstArraySlice = arrayOrDepthSlice;
                            rtvDescription.Texture2DArray.MipSlice = mipIndex;
                        }
                    }
                    else
                    {
                        rtvDescription.Dimension = Description.SampleDescription.Count > 1 ? RenderTargetViewDimension.Texture2DMultisampled : RenderTargetViewDimension.Texture2D;
                        if (Description.SampleDescription.Count <= 1)
                            rtvDescription.Texture2D.MipSlice = mipIndex;
                    }

                    rtv = new TextureView(this, new RenderTargetView(Device, Resource, rtvDescription));
                    renderTargetViews[rtvIndex] = ToDispose(rtv);
                }

                return rtv;
            }
        }

        public override Texture Clone()
        {
            return new RenderTarget2D(Device, Description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D"/> from a <see cref="Texture2DDescription"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description">The description.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static RenderTarget2D New(DirectXDevice device, Texture2DDescription description)
        {
            return new RenderTarget2D(device, description);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D"/> from a <see cref="SharpDX.Direct3D11.Texture2D"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="texture">The native texture <see cref="SharpDX.Direct3D11.Texture2D"/>.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static RenderTarget2D New(DirectXDevice device, SharpDX.Direct3D11.Texture2D texture)
        {
            return new RenderTarget2D(device, texture);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D"/> from a <see cref="RenderTargetView"/>.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="renderTargetView">The native texture <see cref="RenderTargetView"/>.</param>
        /// <returns>
        /// A new instance of <see cref="RenderTarget2D"/> class.
        /// </returns>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        public static RenderTarget2D New(DirectXDevice device, RenderTargetView renderTargetView, bool pureRenderTarget = false)
        {
            using (var resource = renderTargetView.Resource)
            {
                return new RenderTarget2D(device, resource.QueryInterface<SharpDX.Direct3D11.Texture2D>(), renderTargetView, pureRenderTarget);
            }
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D" /> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static RenderTarget2D New(DirectXDevice device, int width, int height, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return New(device, width, height, false, format, flags, arraySize);
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D" />.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static RenderTarget2D New(DirectXDevice device, int width, int height, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return new RenderTarget2D(device, CreateDescription(device, width, height, format, flags, mipCount, arraySize, MSAALevel.None));
        }

        /// <summary>
        /// Creates a new <see cref="RenderTarget2D" /> using multisampling.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="multiSampleCount">The multisample count.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static RenderTarget2D New(DirectXDevice device, int width, int height, MSAALevel multiSampleCount, PixelFormat format, int arraySize = 1)
        {
            if (multiSampleCount == MSAALevel.None)
            {
                throw new ArgumentException("Cannot declare a MSAA RenderTarget with MSAALevel.None. Use other non-MSAA constructors", "multiSampleCount");
            }

            return new RenderTarget2D(device, CreateDescription(device, width, height, format, TextureFlags.RenderTarget, 1, arraySize, multiSampleCount));
        }

        /// <summary>
        /// Creates a new texture description for a <see cref="RenderTarget2D" /> with a single mipmap.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2DDescription CreateDescription(DirectXDevice device, int width, int height, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return CreateDescription(device, width, height, false, format, flags, arraySize);
        }

        /// <summary>
        /// Creates a new texture description <see cref="RenderTarget2D" />.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipCount">Number of mipmaps, set to true to have all mipmaps, set to an int >=1 for a particular mipmap count.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="flags">Sets the texture flags (for unordered access...etc.)</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2DDescription CreateDescription(DirectXDevice device, int width, int height, MipMapCount mipCount, PixelFormat format, TextureFlags flags = TextureFlags.RenderTarget | TextureFlags.ShaderResource, int arraySize = 1)
        {
            return CreateDescription(device, width, height, format, flags, mipCount, arraySize, MSAALevel.None);
        }

        /// <summary>
        /// Creates a new texture description <see cref="RenderTarget2D" /> using multisampling.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">Describes the format to use.</param>
        /// <param name="arraySize">Size of the texture 2D array, default to 1.</param>
        /// <param name="multiSampleCount">The multisample count.</param>
        /// <returns>A new instance of <see cref="RenderTarget2D" /> class.</returns>
        /// <msdn-id>ff476521</msdn-id>
        ///   <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>
        ///   <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>
        public static Texture2DDescription CreateDescription(DirectXDevice device, int width, int height, MSAALevel multiSampleCount, PixelFormat format, int arraySize = 1)
        {
            if (multiSampleCount == MSAALevel.None)
            {
                throw new ArgumentException("Cannot declare a MSAA RenderTarget with MSAALevel.None. Use other non-MSAA constructors", "multiSampleCount");
            }

            return CreateDescription(device, width, height, format, TextureFlags.RenderTarget, 1, arraySize, multiSampleCount);
        }

        /// <summary>
        /// <see cref="SharpDX.Direct3D11.Texture2D"/> casting operator.
        /// </summary>
        /// <param name="from">From the Texture1D.</param>
        public static implicit operator SharpDX.Direct3D11.Texture2D(RenderTarget2D from)
        {
            // Don't bother with multithreading here
            return from == null ? null : from.Resource;
        }

        /// <summary>
        /// Implicit casting operator to <see cref="SharpDX.Direct3D11.Resource"/>
        /// </summary>
        /// <param name="from">The GraphicsResource to convert from.</param>
        public static implicit operator Resource(RenderTarget2D from)
        {
            return from == null ? null : (Resource)from.Resource;
        }

        internal static Texture2DDescription CreateDescription(DirectXDevice device, int width, int height, PixelFormat format, TextureFlags textureFlags, int mipCount, int arraySize, MSAALevel multiSampleCount)
        {
            // Make sure that the texture to create is a render target
            textureFlags |= TextureFlags.RenderTarget;
            var desc = NewDescription(width, height, format, textureFlags, mipCount, arraySize, ResourceUsage.Default);

            // Sets the MSAALevel
            int maximumMSAA = (int)device.Features[format].MSAALevelMax;
            desc.SampleDescription.Count = Math.Max(1, Math.Min((int)multiSampleCount, maximumMSAA));
            return desc;
        }
    }
}
