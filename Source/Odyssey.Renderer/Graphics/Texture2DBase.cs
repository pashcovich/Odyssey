using System.Collections.Generic;
using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Odyssey.Graphics
{
        /// <summary>
    /// Abstract class front end to <see cref="SharpDX.Direct3D11.Texture2D"/>.
    /// </summary>
    public abstract class Texture2DBase : Texture
    {
        protected readonly new SharpDX.Direct3D11.Texture2D Resource;
        private Surface dxgiSurface;

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2DBase" /> class.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description2D">The description.</param>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        protected internal Texture2DBase(DirectXDevice device, Texture2DDescription description2D)
            : base(device, description2D)
        {
            Resource = new SharpDX.Direct3D11.Texture2D(device, description2D);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2DBase" /> class.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="description2D">The description.</param>
        /// <param name="dataBoxes">A variable-length parameters list containing data rectangles.</param>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        protected internal Texture2DBase(DirectXDevice device, Texture2DDescription description2D, DataBox[] dataBoxes)
            : base(device , description2D)
        {
            Resource = new SharpDX.Direct3D11.Texture2D(device, description2D, dataBoxes);
        }

        /// <summary>
        /// Specialised constructor for use only by derived classes.
        /// </summary>
        /// <param name="device">The <see cref="DirectXDevice"/>.</param>
        /// <param name="texture">The texture.</param>
        /// <msdn-id>ff476521</msdn-id>	
        /// <unmanaged>HRESULT ID3D11Device::CreateTexture2D([In] const D3D11_TEXTURE2D_DESC* pDesc,[In, Buffer, Optional] const D3D11_SUBRESOURCE_DATA* pInitialData,[Out, Fast] ID3D11Texture2D** ppTexture2D)</unmanaged>	
        /// <unmanaged-short>ID3D11Device::CreateTexture2D</unmanaged-short>	
        protected internal Texture2DBase(DirectXDevice device, SharpDX.Direct3D11.Texture2D texture)
            : base(device, texture.Description)
        {
            Resource = texture;
        }

        public override void Initialize()
        {
            Initialize(ToDispose(Resource));
        }

        /// <summary>
        /// Return an equivalent staging texture CPU read-writable from this instance.
        /// </summary>
        /// <returns></returns>
        public override Texture ToStaging()
        {
            return new Texture2D(Device, Description.ToStagingDescription());            
        }

        protected virtual Format GetDefaultViewFormat()
        {
            return Description.Format;
        }

        internal override TextureView GetShaderResourceView(Format viewFormat, ViewType viewType, int arrayOrDepthSlice, int mipIndex)
        {
            if ((Description.BindFlags & BindFlags.ShaderResource) == 0)
                return null;

            int arrayCount;
            int mipCount;
            GetViewSliceBounds(viewType, ref arrayOrDepthSlice, ref mipIndex, out arrayCount, out mipCount);

            var textureViewKey = new TextureViewKey(viewFormat, viewType, arrayOrDepthSlice, mipIndex);

            lock (shaderResourceViews)
            {
                TextureView srv;
                // Creates the shader resource view
                if (!shaderResourceViews.TryGetValue(textureViewKey, out srv))
                {
                    // Create the view
                    var srvDescription = new ShaderResourceViewDescription { Format = viewFormat };

                    // Initialize for texture arrays or texture cube
                    if (Description.ArraySize > 1)
                    {
                        // If texture cube
                        if ((Description.OptionFlags & ResourceOptionFlags.TextureCube) != 0)
                        {
                            srvDescription.Dimension = ShaderResourceViewDimension.TextureCube;
                            srvDescription.TextureCube.MipLevels = mipCount;
                            srvDescription.TextureCube.MostDetailedMip = mipIndex;
                        }
                        else
                        {
                            // Else regular Texture2D
                            srvDescription.Dimension = Description.SampleDescription.Count > 1 ? ShaderResourceViewDimension.Texture2DMultisampledArray : ShaderResourceViewDimension.Texture2DArray;

                            // Multisample?
                            if (Description.SampleDescription.Count > 1)
                            {
                                srvDescription.Texture2DMSArray.ArraySize = arrayCount;
                                srvDescription.Texture2DMSArray.FirstArraySlice = arrayOrDepthSlice;
                            }
                            else
                            {
                                srvDescription.Texture2DArray.ArraySize = arrayCount;
                                srvDescription.Texture2DArray.FirstArraySlice = arrayOrDepthSlice;
                                srvDescription.Texture2DArray.MipLevels = mipCount;
                                srvDescription.Texture2DArray.MostDetailedMip = mipIndex;
                            }
                        }
                    }
                    else
                    {
                        srvDescription.Dimension = Description.SampleDescription.Count > 1 ? ShaderResourceViewDimension.Texture2DMultisampled : ShaderResourceViewDimension.Texture2D;
                        if (Description.SampleDescription.Count <= 1)
                        {
                            srvDescription.Texture2D.MipLevels = mipCount;
                            srvDescription.Texture2D.MostDetailedMip = mipIndex;
                        }
                    }

                    srv = new TextureView(this, new ShaderResourceView(Device, Resource, srvDescription));
                    shaderResourceViews.Add(textureViewKey, ToDispose(srv));
                }

                return srv;
            }
        }

        internal override UnorderedAccessView GetUnorderedAccessView(int arrayOrDepthSlice, int mipIndex)
        {
            if ((Description.BindFlags & BindFlags.UnorderedAccess) == 0)
                return null;

            const int arrayCount = 1;

            // Use Full although we are binding to a single array/mimap slice, just to get the correct index
            var uavIndex = GetViewIndex(ViewType.Full, arrayOrDepthSlice, mipIndex);

            lock (unorderedAccessViews)
            {
                var uav = unorderedAccessViews[uavIndex];

                // Creates the unordered access view
                if (uav == null)
                {
                    var uavDescription = new UnorderedAccessViewDescription
                    {
                        Format = Description.Format,
                        Dimension = Description.ArraySize > 1 ? UnorderedAccessViewDimension.Texture2DArray : UnorderedAccessViewDimension.Texture2D
                    };

                    if (Description.ArraySize > 1)
                    {
                        uavDescription.Texture2DArray.ArraySize = arrayCount;
                        uavDescription.Texture2DArray.FirstArraySlice = arrayOrDepthSlice;
                        uavDescription.Texture2DArray.MipSlice = mipIndex;
                    }
                    else
                    {
                        uavDescription.Texture2D.MipSlice = mipIndex;
                    }

                    uav = new UnorderedAccessView(Device, Resource, uavDescription) { Tag = this };
                    unorderedAccessViews[uavIndex] = ToDispose(uav);
                }

                return uav;
            }
        }

        /// <summary>
        /// <see cref="SharpDX.DXGI.Surface"/> casting operator.
        /// </summary>
        /// <param name="from">From the Texture1D.</param>
        public static implicit operator Surface(Texture2DBase from)
        {
            // Don't bother with multithreading here
            return from == null ? null : from.dxgiSurface ?? (from.dxgiSurface = from.ToDispose(from.Resource.QueryInterface<Surface>()));
        }

        protected override void InitializeViews()
        {
            // Creates the shader resource view
            if ((Description.BindFlags & BindFlags.ShaderResource) != 0)
            {
                shaderResourceViews = new Dictionary<TextureViewKey, TextureView>();

                // Pre initialize by default the view on the first array/mipmap
                var viewFormat = GetDefaultViewFormat();
                if(!FormatHelper.IsTypeless(viewFormat))
                {
                    // Only valid for non-typeless viewformat
                    defaultShaderResourceView = GetShaderResourceView(viewFormat, ViewType.Full, 0, 0);
                }
            }

            // Creates the unordered access view
            if ((Description.BindFlags & BindFlags.UnorderedAccess) != 0)
            {
                // Initialize the unordered access views
                unorderedAccessViews = new UnorderedAccessView[GetViewCount()];

                // Pre initialize by default the view on the first array/mipmap
                GetUnorderedAccessView(0, 0);
            }
        }

        protected static Texture2DDescription NewDescription(int width, int height, PixelFormat format, TextureFlags textureFlags, int mipCount, int arraySize, ResourceUsage usage)
        {
            if ((textureFlags & TextureFlags.UnorderedAccess) != 0)
                usage = ResourceUsage.Default;

            var desc = new Texture2DDescription
            {
                               Width = width,
                               Height = height,
                               ArraySize = arraySize,
                               SampleDescription = new SampleDescription(1, 0),
                               BindFlags = GetBindFlagsFromTextureFlags(textureFlags),
                               Format = format,
                               MipLevels = CalculateMipMapCount(mipCount, width, height),
                               Usage = usage,
                               CpuAccessFlags = GetCpuAccessFlagsFromUsage(usage),
                               OptionFlags = ResourceOptionFlags.None
                           };


            // If the texture is a RenderTarget + ShaderResource + MipLevels > 1, then allow for GenerateMipMaps method
            if ((desc.BindFlags & BindFlags.RenderTarget) != 0 && (desc.BindFlags & BindFlags.ShaderResource) != 0 && desc.MipLevels > 1)
            {
                desc.OptionFlags |= ResourceOptionFlags.GenerateMipMaps;
            }

            return desc;
        }
    }
    
}
