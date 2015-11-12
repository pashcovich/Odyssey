#region Using Directives

using Odyssey.Core;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Shaders;
using SharpDX;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using BlendState = Odyssey.Graphics.BlendState;
using Buffer = SharpDX.Direct3D11.Buffer;
using DepthStencilState = Odyssey.Graphics.DepthStencilState;
using Device = SharpDX.Direct3D11.Device;
using Device1 = SharpDX.Direct3D11.Device1;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;
using PixelShader = SharpDX.Direct3D11.PixelShader;
using RasterizerState = Odyssey.Graphics.RasterizerState;
using Resource = SharpDX.Direct3D11.Resource;
using ResultCode = SharpDX.DXGI.ResultCode;
using SamplerState = Odyssey.Graphics.SamplerState;
using VertexShader = SharpDX.Direct3D11.VertexShader;

#endregion Using Directives

namespace Odyssey.Engine
{
    public class DirectXDevice : Component, IDirect3DProvider, ITarget
    {
        internal InputAssemblerStage InputAssembler;
        internal OutputMergerStage OutputMerger;
        internal PixelShaderStage PixelShader;
        internal RasterizerStage RasterizerStage;
        internal VertexShaderStage VertexShader;
        private const int SimultaneousRenderTargetCount = OutputMergerStage.SimultaneousRenderTargetCount;
        private readonly RenderTargetView[] currentRenderTargetViews;
        private readonly DeviceFeatures features;
        private readonly bool isDebugMode;
        private readonly TechniquePool techniquePool;
        private readonly Device1 device;
        private readonly DeviceContext1 context;
        public Device1 NativeDevice => device;
        DeviceContext1 IDirect3DProvider.Context => context;
        Device1 IDirect3DProvider.Device => device;

        private readonly ViewportF[] viewports;

        private int actualRenderTargetViewCount;
        private DepthStencilView currentDepthStencilView;
        private RenderTargetView currentRenderTargetView;

        private Technique currentTechnique;
        private VertexInputLayout currentVertexInputLayout;

        private int maxSlotCountForVertexBuffer;
        protected DirectXDevice(DriverType type, DeviceCreationFlags flags = DeviceCreationFlags.None,
            params FeatureLevel[] featureLevels)
            : this((featureLevels != null && featureLevels.Length > 0)
                ? new Device(type, flags, featureLevels)
                : new Device(type, flags))
        {
        }

        protected DirectXDevice(GraphicsAdapter adapter, DeviceCreationFlags flags = DeviceCreationFlags.None,
            params FeatureLevel[] featureLevels)
            : this((featureLevels != null && featureLevels.Length > 0)
                ? new Device(adapter, flags, featureLevels)
                : new Device(adapter, flags), adapter)
        {
        }

        protected DirectXDevice(Device existingDevice, GraphicsAdapter adapter = null)
        {
            ToDispose(existingDevice);
            device = ToDispose(existingDevice.QueryInterface<Device1>());

            // Get Direct3D 11.1 context
            context = ToDispose(device.ImmediateContext.QueryInterface<DeviceContext1>());

            // If the adapter is null, then try to locate back the adapter
            if (adapter == null)
            {
                try
                {
                    using (var dxgiDevice = device.QueryInterface<SharpDX.DXGI.Device>())
                    {
                        using (var dxgiAdapter = dxgiDevice.Adapter)
                        {
                            var deviceId = dxgiAdapter.Description.DeviceId;

                            foreach (var graphicsAdapter in GraphicsAdapter.Adapters.Where(graphicsAdapter => deviceId == graphicsAdapter.Description.DeviceId))
                            {
                                Adapter = graphicsAdapter;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            isDebugMode = (device.CreationFlags & DeviceCreationFlags.Debug) != 0;
            viewports = new ViewportF[16];
            features = new DeviceFeatures(device);
            AutoViewportFromRenderTargets = true;
            currentRenderTargetViews = new RenderTargetView[SimultaneousRenderTargetCount];

            SamplerStates = ToDispose(new SamplerStateCollection(this));
            RasterizerStates = ToDispose(new RasterizerStateCollection(this));
            DepthStencilStates = ToDispose(new DepthStencilStateCollection(this));
            BlendStates = ToDispose(new BlendStateCollection(this));
            techniquePool = ToDispose(new TechniquePool(this));
            Initialize();
        }

        public GraphicsAdapter Adapter { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the viewport is automatically calculated and set when a render target is set. Default is true.
        /// </summary>
        /// <value><c>true</c> if the viewport is automatically calculated and set when a render target is set; otherwise, <c>false</c>.</value>
        public bool AutoViewportFromRenderTargets { get; set; }

        /// <summary>
        /// Gets the depth stencil buffer sets by the current <see cref="Presenter" /> setup on this device.
        /// </summary>
        /// <value>The depth stencil buffer. The returned value may be null if no <see cref="GraphicsPresenter"/> are setup on this device or no depth buffer was allocated.</value>
        public DepthStencilBuffer DepthStencilBuffer => Presenter != null ? Presenter.DepthStencilBuffer : null;

        /// <summary>
        /// Gets the status of this device.
        /// </summary>
        /// <msdn-id>ff476526</msdn-id>
        /// <unmanaged>GetDeviceRemovedReason</unmanaged>
        /// <unmanaged-short>GetDeviceRemovedReason</unmanaged-short>
        /// <unmanaged>HRESULT ID3D11Device::GetDeviceRemovedReason()</unmanaged>
        public DeviceStatus DeviceStatus
        {
            get
            {
                var result = device.DeviceRemovedReason;
                if (result == ResultCode.DeviceRemoved)
                {
                    return DeviceStatus.Removed;
                }

                if (result == ResultCode.DeviceReset)
                {
                    return DeviceStatus.Reset;
                }

                if (result == ResultCode.DeviceHung)
                {
                    return DeviceStatus.Hung;
                }

                if (result == ResultCode.DriverInternalError)
                {
                    return DeviceStatus.InternalError;
                }

                if (result == ResultCode.InvalidCall)
                {
                    return DeviceStatus.InvalidCall;
                }

                if (result.Code < 0)
                {
                    return DeviceStatus.Reset;
                }

                return DeviceStatus.Normal;
            }
        }

        public DeviceFeatures Features => features;

        public bool IsDebugMode => isDebugMode;



        public GraphicsPresenter Presenter { get; set; }

        /// <summary>
        /// Gets the back buffer sets by the current <see cref="Presenter" /> setup on this device.
        /// </summary>
        /// <value>The back buffer. The returned value may be null if no <see cref="GraphicsPresenter"/> are setup on this device.</value>
        public RenderTarget2D BackBuffer => Presenter?.BackBuffer;

        public TechniquePool TechniquePool => techniquePool;

        /// <summary>
        /// Gets the main viewport.
        /// </summary>
        /// <value>The main viewport.</value>
        public ViewportF Viewport
        {
            get
            {
                RasterizerStage.GetViewports(viewports);
                return viewports[0];
            }

            set { SetViewport(value); }
        }

        internal BlendStateCollection BlendStates { get; private set; }

        internal Technique CurrentTechnique => currentTechnique;

        internal DepthStencilStateCollection DepthStencilStates { get; private set; }

        internal RasterizerStateCollection RasterizerStates { get; private set; }

        internal IntPtr ResetSlotsPointers { get; private set; }

        internal SamplerStateCollection SamplerStates { get; private set; }

        private PrimitiveTopology PrimitiveType
        {
            set { InputAssembler.PrimitiveTopology = value; }
        }

        public static implicit operator Device(DirectXDevice from)
        {
            return @from?.device;
        }

        public static implicit operator DeviceContext(DirectXDevice from)
        {
            return @from?.context;
        }

        /// <summary>
        /// Creates a new <see cref="DirectXDevice"/> from an existing <see cref="SharpDX.Direct3D11.Device"/>.
        /// </summary>
        /// <param name="existingDevice">An existing device.</param>
        /// <returns>A new instance of <see cref="DirectXDevice"/>.</returns>
        public static DirectXDevice New(Device existingDevice)
        {
            return new DirectXDevice(existingDevice);
        }

        /// <summary>
        /// Creates a new <see cref="DirectXDevice"/> using <see cref="DriverType.Hardware"/>.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="featureLevels">The feature levels.</param>
        /// <returns>A new instance of <see cref="DirectXDevice"/></returns>
        public static DirectXDevice New(DeviceCreationFlags flags = DeviceCreationFlags.None, params FeatureLevel[] featureLevels)
        {
            return New(DriverType.Hardware, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new <see cref="DirectXDevice"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="featureLevels">The feature levels.</param>
        /// <returns>A new instance of <see cref="DirectXDevice"/>.</returns>
        public static DirectXDevice New(DriverType type, DeviceCreationFlags flags = DeviceCreationFlags.None,
            params FeatureLevel[] featureLevels)
        {
            if (type == DriverType.Hardware)
                return new DirectXDevice(GraphicsAdapter.Default, flags, featureLevels);

            return new DirectXDevice(type, flags, featureLevels);
        }

        /// <summary>
        /// Creates a new <see cref="DirectXDevice"/>.
        /// </summary>
        /// <param name="adapter">The graphics adapter to use.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="featureLevels">The feature levels.</param>
        /// <returns>A new instance of <see cref="DirectXDevice"/>.</returns>
        public static DirectXDevice New(GraphicsAdapter adapter, DeviceCreationFlags flags = DeviceCreationFlags.None,
            params FeatureLevel[] featureLevels)
        {
            return new DirectXDevice(adapter, flags, featureLevels);
        }

        /// <summary>
        /// Clears the default render target and depth stencil buffer attached to the current <see cref="Presenter"/>.
        /// </summary>
        /// <param name="color">Set this color value in all buffers.</param>
        /// <exception cref="System.InvalidOperationException">Cannot clear without a Presenter set on this instance</exception>
        public void Clear(Color4 color)
        {
            var options = currentRenderTargetView != null ? ClearOptions.Target : (ClearOptions)0;

            if (currentDepthStencilView != null)
            {
                var textureView = currentDepthStencilView.Tag as TextureView;
                DepthStencilBuffer depthStencilBuffer;

                if (textureView == null || (depthStencilBuffer = textureView.Texture as DepthStencilBuffer) == null)
                {
                    throw new InvalidOperationException(
                        "Clear on a custom DepthStencilView is not supported by this method. Use Clear(DepthStencilView) directly");
                }

                options |= depthStencilBuffer.HasStencil
                    ? ClearOptions.DepthBuffer | ClearOptions.Stencil
                    : ClearOptions.DepthBuffer;
            }

            Clear(options, color, 1f, 0);
        }

        /// <summary>
        /// Clears the default render target and depth stencil buffer attached to the current <see cref="Presenter"/>.
        /// </summary>
        /// <param name="options">Options for clearing a buffer.</param>
        /// <param name="color">Set this four-component color value in the buffer.</param>
        /// <param name="depth">Set this depth value in the buffer.</param>
        /// <param name="stencil">Set this stencil value in the buffer.</param>
        public void Clear(ClearOptions options, Color4 color, float depth, int stencil)
        {
            if ((options & ClearOptions.Target) != 0)
            {
                if (currentRenderTargetView == null)
                {
                    throw new InvalidOperationException(
                        "No default render target view setup. Call SetRenderTargets before calling this method.");
                }
                Clear(currentRenderTargetView, color);
            }

            if ((options & (ClearOptions.Stencil | ClearOptions.DepthBuffer)) != 0)
            {
                if (currentDepthStencilView == null)
                {
                    throw new InvalidOperationException(
                        "No default depth stencil view setup. Call SetRenderTargets before calling this method.");
                }

                var flags = (options & ClearOptions.DepthBuffer) != 0 ? DepthStencilClearFlags.Depth : 0;
                if ((options & ClearOptions.Stencil) != 0)
                {
                    flags |= DepthStencilClearFlags.Stencil;
                }

                Clear(currentDepthStencilView, flags, depth, (byte)stencil);
            }
        }

        /// <summary>
        /// Clears a render target view by setting all the elements in a render target to one value.
        /// </summary>
        /// <param name="renderTargetView">The render target view.</param>
        /// <param name="colorRGBA">A 4-component array that represents the color to fill the render target with.</param>
        /// <remarks><p>Applications that wish to clear a render target to a specific integer value bit pattern should render a screen-aligned quad instead of using this method.  The reason for this is because this method accepts as input a floating point value, which may not have the same bit pattern as the original integer.</p><table> <tr><td> <p>Differences between Direct3D 9 and Direct3D 11/10:</p> <p>Unlike Direct3D 9, the full extent of the resource view is always cleared. Viewport and scissor settings are not applied.</p> </td></tr> </table><p>?</p></remarks>
        /// <msdn-id>ff476388</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::ClearRenderTargetView([In] ID3D11RenderTargetView* pRenderTargetView,[In] const SHARPDX_COLOR4* ColorRGBA)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::ClearRenderTargetView</unmanaged-short>
        public void Clear(RenderTargetView renderTargetView, Color4 colorRGBA)
        {
            context.ClearRenderTargetView(renderTargetView, colorRGBA);
        }

        /// <summary>
        /// Clears the depth-stencil resource.
        /// </summary>
        /// <param name="depthStencilView"><dd>  <p>Pointer to the depth stencil to be cleared.</p> </dd></param>
        /// <param name="clearFlags"><dd>  <p>Identify the type of data to clear (see <strong><see cref="SharpDX.Direct3D11.DepthStencilClearFlags"/></strong>).</p> </dd></param>
        /// <param name="depth"><dd>  <p>Clear the depth buffer with this value. This value will be clamped between 0 and 1.</p> </dd></param>
        /// <param name="stencil"><dd>  <p>Clear the stencil buffer with this value.</p> </dd></param>
        /// <remarks>
        /// <table> <tr><td> <p>Differences between Direct3D 9 and Direct3D 11/10:</p> <p>Unlike Direct3D 9, the full extent of the resource view is always cleared. Viewport and scissor settings are not applied.</p> </td></tr> </table><p>?</p>
        /// </remarks>
        /// <msdn-id>ff476387</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::ClearDepthStencilView([In] ID3D11DepthStencilView* pDepthStencilView,[In] D3D11_CLEAR_FLAG ClearFlags,[In] float Depth,[In] unsigned char Stencil)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::ClearDepthStencilView</unmanaged-short>
        public void Clear(DepthStencilView depthStencilView, DepthStencilClearFlags clearFlags, float depth, byte stencil)
        {
            context.ClearDepthStencilView(depthStencilView, clearFlags, depth, stencil);
        }

        /// <summary>
        /// <p>Restore all default settings.</p>
        /// </summary>
        /// <remarks>
        /// <p>This method resets any device context to the default settings. This sets all input/output resource slots, shaders, input layouts, predications, scissor rectangles, depth-stencil state, rasterizer state, blend state, sampler state, and viewports to <strong><c>null</c></strong>. The primitive topology is set to UNDEFINED.</p><p>For a scenario where you would like to clear a list of commands recorded so far, call <strong><see cref="SharpDX.Direct3D11.DeviceContext.FinishCommandListInternal"/></strong> and throw away the resulting <strong><see cref="SharpDX.Direct3D11.CommandList"/></strong>.</p>
        /// </remarks>
        /// <msdn-id>ff476389</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::ClearState()</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::ClearState</unmanaged-short>
        public void ClearState()
        {
            context.ClearState();
        }

        /// <summary>
        /// Copies the content of this resource to another <see cref="GraphicsResource" />.
        /// </summary>
        /// <param name="fromResource">The resource to copy from.</param>
        /// <param name="toResource">The resource to copy to.</param>
        /// <remarks>See the unmanaged documentation for usage and restrictions.</remarks>
        /// <msdn-id>ff476392</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::CopyResource([In] ID3D11Resource* pDstResource,[In] ID3D11Resource* pSrcResource)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::CopyResource</unmanaged-short>
        public void Copy(Resource fromResource, Resource toResource)
        {
            context.CopyResource(fromResource, toResource);
        }

        /// <summary>
        /// Copy a region from a source resource to a destination resource.
        /// </summary>
        /// <remarks>
        /// The source box must be within the size of the source resource. The destination offsets, (x, y, and z) allow the source box to be offset when writing into the destination resource; however, the dimensions of the source box and the offsets must be within the size of the resource. If the resources are buffers, all coordinates are in bytes; if the resources are textures, all coordinates are in texels. {{D3D11CalcSubresource}} is a helper function for calculating subresource indexes. CopySubresourceRegion performs the copy on the GPU (similar to a memcpy by the CPU). As a consequence, the source and destination resources:  Must be different subresources (although they can be from the same resource). Must be the same type. Must have compatible DXGI formats (identical or from the same type group). For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. May not be currently mapped.  CopySubresourceRegion only supports copy; it does not support any stretch, color key, blend, or format conversions. An application that needs to copy an entire resource should use <see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/> instead. CopySubresourceRegion is an asynchronous call which may be added to the command-buffer queue, this attempts to remove pipeline stalls that may occur when copying data. See performance considerations for more details. Note??If you use CopySubresourceRegion with a depth-stencil buffer or a multisampled resource, you must copy the whole subresource. In this situation, you must pass 0 to the DstX, DstY, and DstZ parameters and NULL to the pSrcBox parameter. In addition, source and destination resources, which are represented by the pSrcResource and pDstResource parameters, should have identical sample count values. Example The following code snippet copies a box (located at (120,100),(200,220)) from a source texture into a region (10,20),(90,140) in a destination texture.
        /// <code> D3D11_BOX sourceRegion;
        /// sourceRegion.left = 120;
        /// sourceRegion.right = 200;
        /// sourceRegion.top = 100;
        /// sourceRegion.bottom = 220;
        /// sourceRegion.front = 0;
        /// sourceRegion.back = 1; pd3dDeviceContext-&gt;CopySubresourceRegion( pDestTexture, 0, 10, 20, 0, pSourceTexture, 0, &amp;sourceRegion ); </code>
        ///
        ///  Notice, that for a 2D texture, front and back are set to 0 and 1 respectively.
        /// </remarks>
        /// <param name="source">A reference to the source resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="sourceSubresource">Source subresource index. </param>
        /// <param name="destination">A reference to the destination resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="destinationSubResource">Destination subresource index. </param>
        /// <param name="dstX">The x-coordinate of the upper left corner of the destination region. </param>
        /// <param name="dstY">The y-coordinate of the upper left corner of the destination region. For a 1D subresource, this must be zero. </param>
        /// <param name="dstZ">The z-coordinate of the upper left corner of the destination region. For a 1D or 2D subresource, this must be zero. </param>
        /// <msdn-id>ff476394</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::CopySubresourceRegion([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In] unsigned int DstX,[In] unsigned int DstY,[In] unsigned int DstZ,[In] ID3D11Resource* pSrcResource,[In] unsigned int SrcSubresource,[In, Optional] const D3D11_BOX* pSrcBox)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::CopySubresourceRegion</unmanaged-short>
        public void Copy(Resource source, int sourceSubresource, Resource destination, int destinationSubResource, int dstX = 0,
            int dstY = 0, int dstZ = 0)
        {
            context.CopySubresourceRegion(source, sourceSubresource, null, destination, destinationSubResource, dstX, dstY, dstZ);
        }

        /// <summary>
        /// Copy a region from a source resource to a destination resource.
        /// </summary>
        /// <remarks>
        /// The source box must be within the size of the source resource. The destination offsets, (x, y, and z) allow the source box to be offset when writing into the destination resource; however, the dimensions of the source box and the offsets must be within the size of the resource. If the resources are buffers, all coordinates are in bytes; if the resources are textures, all coordinates are in texels. {{D3D11CalcSubresource}} is a helper function for calculating subresource indexes. CopySubresourceRegion performs the copy on the GPU (similar to a memcpy by the CPU). As a consequence, the source and destination resources:  Must be different subresources (although they can be from the same resource). Must be the same type. Must have compatible DXGI formats (identical or from the same type group). For example, a DXGI_FORMAT_R32G32B32_FLOAT texture can be copied to an DXGI_FORMAT_R32G32B32_UINT texture since both of these formats are in the DXGI_FORMAT_R32G32B32_TYPELESS group. May not be currently mapped.  CopySubresourceRegion only supports copy; it does not support any stretch, color key, blend, or format conversions. An application that needs to copy an entire resource should use <see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/> instead. CopySubresourceRegion is an asynchronous call which may be added to the command-buffer queue, this attempts to remove pipeline stalls that may occur when copying data. See performance considerations for more details. Note??If you use CopySubresourceRegion with a depth-stencil buffer or a multisampled resource, you must copy the whole subresource. In this situation, you must pass 0 to the DstX, DstY, and DstZ parameters and NULL to the pSrcBox parameter. In addition, source and destination resources, which are represented by the pSrcResource and pDstResource parameters, should have identical sample count values. Example The following code snippet copies a box (located at (120,100),(200,220)) from a source texture into a region (10,20),(90,140) in a destination texture.
        /// <code> D3D11_BOX sourceRegion;
        /// sourceRegion.left = 120;
        /// sourceRegion.right = 200;
        /// sourceRegion.top = 100;
        /// sourceRegion.bottom = 220;
        /// sourceRegion.front = 0;
        /// sourceRegion.back = 1; pd3dDeviceContext-&gt;CopySubresourceRegion( pDestTexture, 0, 10, 20, 0, pSourceTexture, 0, &amp;sourceRegion ); </code>
        ///
        ///  Notice, that for a 2D texture, front and back are set to 0 and 1 respectively.
        /// </remarks>
        /// <param name="source">A reference to the source resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="sourceSubresource">Source subresource index. </param>
        /// <param name="sourceRegion">A reference to a 3D box (see <see cref="SharpDX.Direct3D11.ResourceRegion"/>) that defines the source subresources that can be copied. If NULL, the entire source subresource is copied. The box must fit within the source resource. </param>
        /// <param name="destination">A reference to the destination resource (see <see cref="SharpDX.Direct3D11.Resource"/>). </param>
        /// <param name="destinationSubResource">Destination subresource index. </param>
        /// <param name="dstX">The x-coordinate of the upper left corner of the destination region. </param>
        /// <param name="dstY">The y-coordinate of the upper left corner of the destination region. For a 1D subresource, this must be zero. </param>
        /// <param name="dstZ">The z-coordinate of the upper left corner of the destination region. For a 1D or 2D subresource, this must be zero. </param>
        /// <msdn-id>ff476394</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::CopySubresourceRegion([In] ID3D11Resource* pDstResource,[In] unsigned int DstSubresource,[In] unsigned int DstX,[In] unsigned int DstY,[In] unsigned int DstZ,[In] ID3D11Resource* pSrcResource,[In] unsigned int SrcSubresource,[In, Optional] const D3D11_BOX* pSrcBox)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::CopySubresourceRegion</unmanaged-short>
        public void Copy(Resource source, int sourceSubresource, ResourceRegion sourceRegion, Resource destination,
            int destinationSubResource, int dstX = 0, int dstY = 0, int dstZ = 0)
        {
            context.CopySubresourceRegion(source, sourceSubresource, sourceRegion, destination, destinationSubResource, dstX, dstY,
                dstZ);
        }

        /// <summary>
        /// Copy a multisampled resource into a non-multisampled resource.
        /// </summary>
        /// <remarks>
        /// This API is most useful when re-using the resulting render target of one render pass as an input to a second render pass. The source and destination resources must be the same resource type and have the same dimensions. In addition, they must have compatible formats. There are three scenarios for this:  ScenarioRequirements Source and destination are prestructured and typedBoth the source and destination must have identical formats and that format must be specified in the Format parameter. One resource is prestructured and typed and the other is prestructured and typelessThe typed resource must have a format that is compatible with the typeless resource (i.e. the typed resource is DXGI_FORMAT_R32_FLOAT and the typeless resource is DXGI_FORMAT_R32_TYPELESS). The format of the typed resource must be specified in the Format parameter. Source and destination are prestructured and typelessBoth the source and destination must have the same typeless format (i.e. both must have DXGI_FORMAT_R32_TYPELESS), and the Format parameter must specify a format that is compatible with the source and destination (i.e. if both are DXGI_FORMAT_R32_TYPELESS then DXGI_FORMAT_R32_FLOAT could be specified in the Format parameter). For example, given the DXGI_FORMAT_R16G16B16A16_TYPELESS format:  The source (or dest) format could be DXGI_FORMAT_R16G16B16A16_UNORM The dest (or source) format could be DXGI_FORMAT_R16G16B16A16_FLOAT    ?
        /// </remarks>
        /// <param name="source">Source resource. Must be multisampled. </param>
        /// <param name="sourceSubresource">&gt;The source subresource of the source resource. </param>
        /// <param name="destination">Destination resource. Must be a created with the <see cref="SharpDX.Direct3D11.ResourceUsage.Default"/> flag and be single-sampled. See <see cref="SharpDX.Direct3D11.Resource"/>. </param>
        /// <param name="destinationSubresource">A zero-based index, that identifies the destination subresource. Use {{D3D11CalcSubresource}} to calculate the index. </param>
        /// <param name="format">A <see cref="SharpDX.DXGI.Format"/> that indicates how the multisampled resource will be resolved to a single-sampled resource.  See remarks. </param>
        /// <unmanaged>void ID3D11DeviceContext::ResolveSubresource([In] ID3D11Resource* pDstResource,[In] int DstSubresource,[In] ID3D11Resource* pSrcResource,[In] int SrcSubresource,[In] DXGI_FORMAT Format)</unmanaged>
        public void Copy(Resource source, int sourceSubresource, Resource destination, int destinationSubresource, Format format)
        {
            context.ResolveSubresource(source, sourceSubresource, destination, destinationSubresource, format);
        }

        /// <summary>
        /// <p>Copies data from a buffer holding variable length data.</p>
        /// </summary>
        /// <param name="sourceView"><dd>  <p>Pointer to an <strong><see cref="SharpDX.Direct3D11.UnorderedAccessView"/></strong> of a Structured Buffer resource created with either  <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Append"/></strong> or <strong><see cref="SharpDX.Direct3D11.UnorderedAccessViewBufferFlags.Counter"/></strong> specified  when the UAV was created.   These types of resources have hidden counters tracking "how many" records have  been written.</p> </dd></param>
        /// <param name="destinationBuffer"><dd>  <p>Pointer to <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong>.  This can be any buffer resource that other copy commands,  such as <strong><see cref="SharpDX.Direct3D11.DeviceContext.CopyResource_"/></strong> or <strong><see cref="SharpDX.Direct3D11.DeviceContext.CopySubresourceRegion_"/></strong>, are able to write to.</p> </dd></param>
        /// <param name="offsetInBytes"><dd>  <p>Offset from the start of <em>pDstBuffer</em> to write 32-bit UINT structure (vertex) count from <em>pSrcView</em>.</p> </dd></param>
        /// <msdn-id>ff476393</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::CopyStructureCount([In] ID3D11Buffer* pDstBuffer,[In] unsigned int DstAlignedByteOffset,[In] ID3D11UnorderedAccessView* pSrcView)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::CopyStructureCount</unmanaged-short>
        public void CopyCount(UnorderedAccessView sourceView, Buffer destinationBuffer, int offsetInBytes = 0)
        {
            context.CopyStructureCount(destinationBuffer, offsetInBytes, sourceView);
        }

        /// <summary>
        /// <p>Draw non-indexed, non-instanced primitives.</p>
        /// </summary>
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="vertexCount"><dd>  <p>Number of vertices to draw.</p> </dd></param>
        /// <param name="startVertexLocation"><dd>  <p>Index of the first vertex, which is usually an offset in a vertex buffer; it could also be used as the first vertex id generated for a shader parameter marked with the <strong>SV_TargetId</strong> system-value semantic.</p> </dd></param>
        /// <remarks>
        /// <p>A draw API submits work to the rendering pipeline.</p><p>The vertex data for a draw call normally comes from a vertex buffer that is bound to the pipeline. However, you could also provide the vertex data from a shader that has vertex data marked with the <strong>SV_VertexId</strong> system-value semantic.</p>
        /// </remarks>
        /// <msdn-id>ff476407</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::Draw([In] unsigned int VertexCount,[In] unsigned int StartVertexLocation)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Draw</unmanaged-short>
        public void Draw(PrimitiveType primitiveType, int vertexCount, int startVertexLocation = 0)
        {
            PrimitiveType = primitiveType;
            context.Draw(vertexCount, startVertexLocation);
        }

        /// <summary>
        /// <p>Draw indexed, non-instanced primitives.</p>
        /// </summary>
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="indexCount"><dd>  <p>Number of indices to draw.</p> </dd></param>
        /// <param name="startIndexLocation"><dd>  <p>The location of the first index read by the GPU from the index buffer.</p> </dd></param>
        /// <param name="baseVertexLocation"><dd>  <p>A value added to each index before reading a vertex from the vertex buffer.</p> </dd></param>
        /// <remarks>
        /// <p>A draw API submits work to the rendering pipeline.</p><p>If the sum of both indices is negative, the result of the function call is undefined.</p>
        /// </remarks>
        /// <msdn-id>ff476409</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexed([In] unsigned int IndexCount,[In] unsigned int StartIndexLocation,[In] int BaseVertexLocation)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexed</unmanaged-short>
        public void DrawIndexed(PrimitiveType primitiveType, int indexCount, int startIndexLocation = 0,
            int baseVertexLocation = 0)
        {
            PrimitiveType = primitiveType;
            context.DrawIndexed(indexCount, startIndexLocation, baseVertexLocation);
        }

        /// <summary>
        /// <p>Draw indexed, instanced primitives.</p>
        /// </summary>
        /// <param name="primitiveType">Type of the primitive to draw.</param>
        /// <param name="indexCountPerInstance"><dd>  <p>Number of indices read from the index buffer for each instance.</p> </dd></param>
        /// <param name="instanceCount"><dd>  <p>Number of instances to draw.</p> </dd></param>
        /// <param name="startIndexLocation"><dd>  <p>The location of the first index read by the GPU from the index buffer.</p> </dd></param>
        /// <param name="baseVertexLocation"><dd>  <p>A value added to each index before reading a vertex from the vertex buffer.</p> </dd></param>
        /// <param name="startInstanceLocation"><dd>  <p>A value added to each index before reading per-instance data from a vertex buffer.</p> </dd></param>
        /// <remarks>
        /// <p>A draw API submits work to the rendering pipeline.</p><p>Instancing may extend performance by reusing the same geometry to draw multiple objects in a scene. One example of instancing could be  to draw the same object with different positions and colors. Indexing requires multiple vertex buffers: at least one for per-vertex data  and a second buffer for per-instance data.</p>
        /// </remarks>
        /// <msdn-id>ff476410</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::DrawIndexedInstanced([In] unsigned int IndexCountPerInstance,[In] unsigned int InstanceCount,[In] unsigned int StartIndexLocation,[In] int BaseVertexLocation,[In] unsigned int StartInstanceLocation)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::DrawIndexedInstanced</unmanaged-short>
        public void DrawIndexedInstanced(PrimitiveType primitiveType, int indexCountPerInstance, int instanceCount,
            int startIndexLocation = 0, int baseVertexLocation = 0, int startInstanceLocation = 0)
        {
            PrimitiveType = primitiveType;
            context.DrawIndexedInstanced(indexCountPerInstance, instanceCount, startIndexLocation, baseVertexLocation,
                startInstanceLocation);
        }

        /// <summary>
        /// <p>Sends queued-up commands in the command buffer to the graphics processing unit (GPU).</p>
        /// </summary>
        /// <remarks>
        /// <p>Most applications don't need to call this method. If an application calls this method when not necessary, it incurs a performance penalty. Each call to <strong>Flush</strong> incurs a significant amount of overhead.</p><p>When Microsoft Direct3D state-setting, present, or draw commands are called by an application, those commands are queued into an internal command buffer.  <strong>Flush</strong> sends those commands to the GPU for processing. Typically, the Direct3D runtime sends these commands to the GPU automatically whenever the runtime determines that  they need to be sent, such as when the command buffer is full or when an application maps a resource. <strong>Flush</strong> sends the commands manually.</p><p>We recommend that you use <strong>Flush</strong> when the CPU waits for an arbitrary amount of time (such as when  you call the <strong>Sleep</strong> function).</p><p>Because <strong>Flush</strong> operates asynchronously,  it can return either before or after the GPU finishes executing the queued graphics commands. However, the graphics commands eventually always complete. You can call the <strong><see cref="SharpDX.Direct3D11.Device.CreateQuery"/></strong> method with the <strong><see cref="SharpDX.Direct3D11.QueryType.Event"/></strong> value to create an event query; you can then use that event query in a call to the <strong><see cref="SharpDX.Direct3D11.DeviceContext.GetDataInternal"/></strong> method to determine when the GPU is finished processing the graphics commands.
        /// </p><p>Microsoft Direct3D?11 defers the destruction of objects. Therefore, an application can't rely upon objects immediately being destroyed. By calling <strong>Flush</strong>, you destroy any  objects whose destruction was deferred.  If an application requires synchronous destruction of an object, we recommend that the application release all its references, call <strong><see cref="SharpDX.Direct3D11.DeviceContext.ClearState"/></strong>, and then call <strong>Flush</strong>.</p>Deferred Destruction Issues with Flip Presentation Swap Chains<p>Direct3D?11 defers the destruction of objects like views and resources until it can efficiently destroy them. This deferred destruction can cause problems with flip presentation model swap chains. Flip presentation model swap chains have the <strong>DXGI_SWAP_EFFECT_FLIP_SEQUENTIAL</strong> flag set. When you create a flip presentation model swap chain, you can associate only one swap chain at a time with an <strong><see cref="System.IntPtr"/></strong>, <strong>IWindow</strong>, or composition surface. If an application attempts to destroy a flip presentation model swap chain and replace it with another swap chain, the original swap chain is not destroyed when the application immediately frees all of the original swap chain's references.</p><p>Most applications typically use the <strong><see cref="SharpDX.DXGI.SwapChain.ResizeBuffers"/></strong> method for the majority of scenarios where they replace new swap chain buffers for old swap chain buffers. However, if an application must actually destroy an old swap chain and create a new swap chain, the application must force the destruction of all objects that the application freed. To force the destruction, call <strong><see cref="SharpDX.Direct3D11.DeviceContext.ClearState"/></strong> (or otherwise ensure no views are bound to pipeline state), and then call <strong>Flush</strong> on the immediate context. You must force destruction before you call <strong>IDXGIFactory2::CreateSwapChainForHwnd</strong>, <strong>IDXGIFactory2::CreateSwapChainForImmersiveWindow</strong>, or <strong>IDXGIFactory2::CreateSwapChainForCompositionSurface</strong> again to create a new swap chain.</p>
        /// </remarks>
        /// <msdn-id>ff476425</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::Flush()</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::Flush</unmanaged-short>
        public void Flush()
        {
            context.Flush();
        }

        /// <summary>
        /// Gets the viewport.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>Returns a viewport bound to a specified render target</returns>
        public ViewportF GetViewport(int index)
        {
            RasterizerStage.GetViewports(viewports);
            return viewports[index];
        }

        /// <summary>
        /// Presents the Backbuffer to the screen.
        /// </summary>
        /// <remarks>
        /// This method is only working if a <see cref="GraphicsPresenter"/> is set on this device using <see cref="Presenter"/> property.
        /// </remarks>
        /// <msdn-id>bb174576</msdn-id>
        /// <unmanaged>HRESULT IDXGISwapChain::Present([In] unsigned int SyncInterval,[In] DXGI_PRESENT_FLAGS Flags)</unmanaged>
        /// <unmanaged-short>IDXGISwapChain::Present</unmanaged-short>
        public void Present()
        {
            if (Presenter != null)
            {
                try
                {
                    Presenter.Present();
                }
                catch (SharpDXException ex)
                {
                    if (ex.ResultCode == ResultCode.DeviceReset || ex.ResultCode == ResultCode.DeviceRemoved)
                    {
                        // TODO: Implement device reset / removed
                    }
                    throw;
                }
            }
        }

        /// <summary>
        /// Resets all vertex buffers bounded to a slot range. By default, It clears all the bounded buffers. See remarks.
        /// </summary>
        /// <remarks>
        /// This is sometimes required to unding explicitly vertex buffers bounding to the input shader assembly, when a
        /// vertex buffer is used as the output of the pipeline.
        /// </remarks>
        /// <msdn-id>ff476456</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>
        public void ResetVertexBuffers()
        {
            if (maxSlotCountForVertexBuffer == 0)
                return;

            InputAssembler.SetVertexBuffers(0, maxSlotCountForVertexBuffer, ResetSlotsPointers, ResetSlotsPointers,
                ResetSlotsPointers);

            maxSlotCountForVertexBuffer = 0;
        }

        /// <summary>
        /// <p>Sets the blend state of the output-merger stage.</p>
        /// </summary>
        /// <param name="blendState"><dd>  <p>Pointer to a blend-state interface (see <strong><see cref="SharpDX.Direct3D11.BlendState"/></strong>). Passing in <strong><c>null</c></strong> implies a default blend state. See remarks for further details.</p> </dd></param>
        /// <remarks>
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>
        /// </remarks>
        /// <msdn-id>ff476462</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::OMSetBlendState</unmanaged-short>
        public void SetBlendState(BlendState blendState)
        {
            if (blendState == null)
            {
                OutputMerger.SetBlendState(null, Color.White, -1);
            }
            else
            {
                OutputMerger.SetBlendState(blendState, blendState.BlendFactor, blendState.MultiSampleMask);
            }
        }

        /// <summary>
        /// <p>Sets the blend state of the output-merger stage.</p>
        /// </summary>
        /// <param name="blendState"><dd>  <p>Pointer to a blend-state interface (see <strong><see cref="SharpDX.Direct3D11.BlendState"/></strong>). Passing in <strong><c>null</c></strong> implies a default blend state. See remarks for further details.</p> </dd></param>
        /// <param name="blendFactor"><dd>  <p>Array of blend factors, one for each RGBA component. This requires a blend state object that specifies the <strong><see cref="SharpDX.Direct3D11.BlendOption.BlendFactor"/></strong> option.</p> </dd></param>
        /// <param name="multiSampleMask"><dd>  <p>32-bit sample coverage. The default value is 0xffffffff. See remarks.</p> </dd></param>
        /// <remarks>
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>
        /// </remarks>
        /// <msdn-id>ff476462</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::OMSetBlendState</unmanaged-short>
        public void SetBlendState(BlendState blendState, Color4 blendFactor, int multiSampleMask = -1)
        {
            if (blendState == null)
            {
                OutputMerger.SetBlendState(null, blendFactor, multiSampleMask);
            }
            else
            {
                OutputMerger.SetBlendState(blendState, blendFactor, multiSampleMask);
            }
        }

        /// <summary>
        /// <p>Sets the blend state of the output-merger stage.</p>
        /// </summary>
        /// <param name="blendState"><dd>  <p>Pointer to a blend-state interface (see <strong><see cref="SharpDX.Direct3D11.BlendState"/></strong>). Passing in <strong><c>null</c></strong> implies a default blend state. See remarks for further details.</p> </dd></param>
        /// <param name="blendFactor"><dd>  <p>Array of blend factors, one for each RGBA component. This requires a blend state object that specifies the <strong><see cref="SharpDX.Direct3D11.BlendOption.BlendFactor"/></strong> option.</p> </dd></param>
        /// <param name="multiSampleMask"><dd>  <p>32-bit sample coverage. The default value is 0xffffffff. See remarks.</p> </dd></param>
        /// <remarks>
        /// <p>Blend state is used by the output-merger stage to determine how to blend together two pixel values. The two values are commonly the current pixel value and the pixel value already in the output render target. Use the <strong>blend operation</strong> to control where the two pixel values come from and how they are mathematically combined.</p><p>To create a blend-state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateBlendState"/></strong>.</p><p>Passing in <strong><c>null</c></strong> for the blend-state interface indicates to the runtime to set a default blending state.  The following table indicates the default blending parameters.</p><table> <tr><th>State</th><th>Default Value</th></tr> <tr><td>AlphaToCoverageEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong></td></tr> <tr><td>BlendEnable</td><td><strong><see cref="SharpDX.Result.False"/></strong>[8]</td></tr> <tr><td>SrcBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlend</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOp</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>SrcBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.One"/></td></tr> <tr><td>DstBlendAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOption.Zero"/></td></tr> <tr><td>BlendOpAlpha</td><td><see cref="SharpDX.Direct3D11.BlendOperation.Add"/></td></tr> <tr><td>RenderTargetWriteMask[8]</td><td><see cref="SharpDX.Direct3D11.ColorWriteMaskFlags.All"/>[8]</td></tr> </table><p>?</p><p>A sample mask determines which samples get updated in all the active render targets. The mapping of bits in a sample mask to samples in a multisample render target is the responsibility of an individual application. A sample mask is always applied; it is independent of whether multisampling is enabled, and does not depend on whether an application uses multisample render targets.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>
        /// </remarks>
        /// <msdn-id>ff476462</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::OMSetBlendState([In, Optional] ID3D11BlendState* pBlendState,[In, Optional] const SHARPDX_COLOR4* BlendFactor,[In] unsigned int SampleMask)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::OMSetBlendState</unmanaged-short>
        public void SetBlendState(BlendState blendState, Color4 blendFactor, uint multiSampleMask = 0xFFFFFFFF)
        {
            SetBlendState(blendState, blendFactor, unchecked((int)multiSampleMask));
        }

        /// <summary>
        /// Sets the depth-stencil state of the output-merger stage.
        /// </summary>
        /// <param name="depthStencilState"><dd>  <p>Pointer to a depth-stencil state interface (see <strong><see cref="SharpDX.Direct3D11.DepthStencilState"/></strong>) to bind to the device. Set this to <strong><c>null</c></strong> to use the default state listed in <strong><see cref="SharpDX.Direct3D11.DepthStencilStateDescription"/></strong>.</p> </dd></param>
        /// <param name="stencilReference"><dd>  <p>Reference value to perform against when doing a depth-stencil test. See remarks.</p> </dd></param>
        /// <remarks>
        /// <p>To create a depth-stencil state interface, call <strong><see cref="SharpDX.Direct3D11.Device.CreateDepthStencilState"/></strong>.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>
        /// </remarks>
        /// <msdn-id>ff476463</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::OMSetDepthStencilState([In, Optional] ID3D11DepthStencilState* pDepthStencilState,[In] unsigned int StencilRef)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::OMSetDepthStencilState</unmanaged-short>
        public void SetDepthStencilState(DepthStencilState depthStencilState, int stencilReference = 0)
        {
            OutputMerger.SetDepthStencilState(depthStencilState, stencilReference);
        }

        /// <summary>
        /// <p>Bind an index buffer to the input-assembler stage.</p>
        /// </summary>
        /// <param name="indexBuffer"><dd>  <p>A reference to an <strong><see cref="SharpDX.Direct3D11.Buffer"/></strong> object, that contains indices. The index buffer must have been created with  the <strong><see cref="SharpDX.Direct3D11.BindFlags.IndexBuffer"/></strong> flag.</p> </dd></param>
        /// <param name="is32Bit">Set to true if indices are 32-bit values (integer size) or false if they are 16-bit values (short size)</param>
        /// <param name="offset">Offset (in bytes) from the start of the index buffer to the first index to use. Default to 0</param>
        /// <remarks>
        /// <p>For information about creating index buffers, see How to: Create an Index Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind  <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will  not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.</p>
        /// </remarks>
        /// <msdn-id>ff476453</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::IASetIndexBuffer([In, Optional] ID3D11Buffer* pIndexBuffer,[In] DXGI_FORMAT Format,[In] unsigned int Offset)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::IASetIndexBuffer</unmanaged-short>
        public void SetIndexBuffer(Buffer indexBuffer, bool is32Bit, int offset = 0)
        {
            InputAssembler.SetIndexBuffer(indexBuffer, is32Bit ? Format.R32_UInt : Format.R16_UInt, offset);
        }

        public void SetPixelShaderConstantBuffer(int slot, Buffer constantBuffer)
        {
            PixelShader.SetConstantBuffer(slot, constantBuffer);
        }

        public void SetPixelShaderSampler(int slot, SamplerState samplerState)
        {
            PixelShader.SetSampler(0, samplerState);
        }

        public void SetPixelShaderShaderResourceView(int slot, ShaderResourceView shaderResourceView)
        {
            PixelShader.SetShaderResource(slot, shaderResourceView);
        }

        /// <summary>
        /// <p>Sets the <strong>rasterizer state</strong> for the rasterizer stage of the pipeline.</p>
        /// </summary>
        /// <param name="rasterizerState">The rasterizer state to set on this device.</param>
        /// <msdn-id>ff476479</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::RSSetState([In, Optional] ID3D11RasterizerState* pRasterizerState)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::RSSetState</unmanaged-short>
        public void SetRasterizerState(RasterizerState rasterizerState)
        {
            RasterizerStage.State = rasterizerState;
        }

        /// <summary>
        /// Binds a depth-stencil buffer and a set of render targets to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetViews">A set of render target views to bind.</param>
        /// <remarks>
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>
        /// </remarks>
        /// <msdn-id>ff476464</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>
        public void SetRenderTargets(DepthStencilView depthStencilView, params RenderTargetView[] renderTargetViews)
        {
            Contract.Requires<ArgumentNullException>(renderTargetViews != null, "renderTargetViews is null");

            CommonSetRenderTargets(renderTargetViews);
            currentDepthStencilView = depthStencilView;
            OutputMerger.SetTargets(depthStencilView, renderTargetViews);
        }

        /// <summary>
        /// Binds a depth-stencil buffer and a single render target to the output-merger stage.
        /// </summary>
        /// <param name = "depthStencilView">A view of the depth-stencil buffer to bind.</param>
        /// <param name = "renderTargetView">A view of the render target to bind.</param>
        /// <remarks>
        /// <p>The maximum number of active render targets a device can have active at any given time is set by a #define in D3D11.h called  D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT. It is invalid to try to set the same subresource to multiple render target slots.  Any render targets not defined by this call are set to <strong><c>null</c></strong>.</p><p>If any subresources are also currently bound for reading in a different stage or writing (perhaps in a different part of the pipeline),  those bind points will be set to <strong><c>null</c></strong>, in order to prevent the same subresource from being read and written simultaneously in a single rendering operation.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p><p>If the render-target views were created from an array resource type, then all of the render-target views must have the same array size.   This restriction also applies to the depth-stencil view, its array size must match that of the render-target views being bound.</p><p>The pixel shader must be able to simultaneously render to at least eight separate render targets. All of these render targets must access the same type of resource: Buffer, Texture1D, Texture1DArray, Texture2D, Texture2DArray, Texture3D, or TextureCube. All render targets must have the same size in all dimensions (width and height, and depth for 3D or array size for *Array types). If render targets use multisample anti-aliasing, all bound render targets and depth buffer must be the same form of multisample resource (that is, the sample counts must be the same). Each render target can have a different data format. These render target formats are not required to have identical bit-per-element counts.</p><p>Any combination of the eight slots for render targets can have a render target set or not set.</p><p>The same resource view cannot be bound to multiple render target slots simultaneously. However, you can set multiple non-overlapping resource views of a single resource as simultaneous multiple render targets.</p>
        /// </remarks>
        /// <msdn-id>ff476464</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::OMSetRenderTargets([In] unsigned int NumViews,[In] const void** ppRenderTargetViews,[In, Optional] ID3D11DepthStencilView* pDepthStencilView)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::OMSetRenderTargets</unmanaged-short>
        public void SetRenderTargets(DepthStencilView depthStencilView, RenderTargetView renderTargetView)
        {
            CommonSetRenderTargets(renderTargetView);
            currentDepthStencilView = depthStencilView;
            OutputMerger.SetTargets(depthStencilView, renderTargetView);
        }

        /// <summary>
        /// Binds a single scissor rectangle to the rasterizer stage.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <param name="right">The right.</param>
        /// <param name="bottom">The bottom.</param>
        /// <remarks>
        /// <p>All scissor rects must be set atomically as one operation. Any scissor rects not defined by the call are disabled.</p><p>The scissor rectangles will only be used if ScissorEnable is set to true in the rasterizer state (see <strong><see cref="SharpDX.Direct3D11.RasterizerStateDescription"/></strong>).</p><p>Which scissor rectangle to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader (see shader semantic syntax). If a geometry shader does not make use of the SV_ViewportArrayIndex semantic then Direct3D will use the first scissor rectangle in the array.</p><p>Each scissor rectangle in the array corresponds to a viewport in an array of viewports (see <strong><see cref="SharpDX.Direct3D11.RasterizerStage.SetViewports"/></strong>).</p>
        /// </remarks>
        /// <msdn-id>ff476478</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer, Optional] const void* pRects)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::RSSetScissorRects</unmanaged-short>
        public void SetScissorRectangles(int left, int top, int right, int bottom)
        {
            RasterizerStage.SetScissorRectangle(left, top, right, bottom);
        }

        /// <summary>
        /// Binds a set of scissor rectangles to the rasterizer stage.
        /// </summary>
        /// <param name = "scissorRectangles">The set of scissor rectangles to bind.</param>
        /// <remarks>
        /// <p>All scissor rects must be set atomically as one operation. Any scissor rects not defined by the call are disabled.</p><p>The scissor rectangles will only be used if ScissorEnable is set to true in the rasterizer state (see <strong><see cref="SharpDX.Direct3D11.RasterizerStateDescription"/></strong>).</p><p>Which scissor rectangle to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader (see shader semantic syntax). If a geometry shader does not make use of the SV_ViewportArrayIndex semantic then Direct3D will use the first scissor rectangle in the array.</p><p>Each scissor rectangle in the array corresponds to a viewport in an array of viewports (see <strong><see cref="SharpDX.Direct3D11.RasterizerStage.SetViewports"/></strong>).</p>
        /// </remarks>
        /// <msdn-id>ff476478</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::RSSetScissorRects([In] unsigned int NumRects,[In, Buffer, Optional] const void* pRects)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::RSSetScissorRects</unmanaged-short>
        public void SetScissorRectangles(params Rectangle[] scissorRectangles)
        {
            RasterizerStage.SetScissorRectangles(scissorRectangles);
        }

        public void SetShader(VertexShader shader)
        {
            VertexShader.Set(shader);
        }

        public void SetShader(PixelShader shader)
        {
            PixelShader.Set(shader);
        }

        /// <summary>
        /// Bind a vertex buffer on the slot #0 of the input-assembler stage.
        /// </summary>
        /// <param name="vertexBuffer">The vertex buffer to bind to this slot. This vertex buffer must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</param>
        /// <param name="vertexIndex">The index is the number of vertex element between the first element of a vertex buffer and the first element that will be used.</param>
        /// <remarks>
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10. </p>
        /// </remarks>
        /// <msdn-id>ff476456</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>
        public void SetVertexBuffer<T>(Buffer<T> vertexBuffer, int vertexIndex = 0) where T : struct
        {
            SetVertexBuffer(0, vertexBuffer, vertexIndex);
        }

        /// <summary>
        /// Bind a vertex buffer to the input-assembler stage.
        /// </summary>
        /// <param name="slot">The first input slot for binding.</param>
        /// <param name="vertexBuffer">The vertex buffer to bind to this slot. This vertex buffer must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</param>
        /// <param name="vertexIndex">The index is the number of vertex element between the first element of a vertex buffer and the first element that will be used.</param>
        /// <remarks>
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.</p>
        /// </remarks>
        /// <msdn-id>ff476456</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>
        public unsafe void SetVertexBuffer<T>(int slot, Buffer<T> vertexBuffer, int vertexIndex = 0) where T : struct
        {
            IntPtr vertexBufferPtr = IntPtr.Zero;
            int stride = SharpDX.Utilities.SizeOf<T>();
            int offset = vertexIndex * stride;
            if (vertexBuffer != null)
            {
                vertexBufferPtr = ((Buffer)vertexBuffer).NativePointer;

                // Update the index of the last slot buffer bounded, used by ResetVertexBuffers
                if ((slot + 1) > maxSlotCountForVertexBuffer)
                    maxSlotCountForVertexBuffer = slot + 1;
            }
            InputAssembler.SetVertexBuffers(slot, 1, new IntPtr(&vertexBufferPtr), new IntPtr(&stride), new IntPtr(&offset));
        }

        /// <summary>
        /// <p>Bind a vertex buffer to the input-assembler stage.</p>
        /// </summary>
        /// <param name="slot">The first input slot for binding.</param>
        /// <param name="vertexBuffer">The vertex buffer to bind to this slot. This vertex buffer must have been created with the <strong><see cref="SharpDX.Direct3D11.BindFlags.VertexBuffer"/></strong> flag.</param>
        /// <param name="vertexStride">The vertexStride is the size (in bytes) of the elements that are to be used from that vertex buffer.</param>
        /// <param name="offsetInBytes">The offset is the number of bytes between the first element of a vertex buffer and the first element that will be used.</param>
        /// <remarks>
        /// <p>For information about creating vertex buffers, see Create a Vertex Buffer.</p><p>Calling this method using a buffer that is currently bound for writing (i.e. bound to the stream output pipeline stage) will effectively bind <strong><c>null</c></strong> instead because a buffer cannot be bound as both an input and an output at the same time.</p><p>The debug layer will generate a warning whenever a resource is prevented from being bound simultaneously as an input and an output, but this will not prevent invalid data from being used by the runtime.</p><p> The method will hold a reference to the interfaces passed in. This differs from the device state behavior in Direct3D 10.</p>
        /// </remarks>
        /// <msdn-id>ff476456</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::IASetVertexBuffers([In] unsigned int StartSlot,[In] unsigned int NumBuffers,[In, Buffer] const void* ppVertexBuffers,[In, Buffer] const void* pStrides,[In, Buffer] const void* pOffsets)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::IASetVertexBuffers</unmanaged-short>
        public unsafe void SetVertexBuffer(int slot, Buffer vertexBuffer, int vertexStride, int offsetInBytes = 0)
        {
            IntPtr vertexBufferPtr = IntPtr.Zero;
            if (vertexBuffer != null)
            {
                vertexBufferPtr = vertexBuffer.NativePointer;

                // Update the index of the last slot buffer bounded, used by ResetVertexBuffers
                if ((slot + 1) > maxSlotCountForVertexBuffer)
                    maxSlotCountForVertexBuffer = slot + 1;
            }
            InputAssembler.SetVertexBuffers(slot, 1, new IntPtr(&vertexBufferPtr), new IntPtr(&vertexStride),
                new IntPtr(&offsetInBytes));
        }

        /// <summary>
        /// Sets the vertex input layout.
        /// </summary>
        /// <param name="inputLayout">The input layout.</param>
        /// <msdn-id>ff476454</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::IASetInputLayout([In, Optional] ID3D11InputLayout* pInputLayout)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::IASetInputLayout</unmanaged-short>
        public void SetVertexInputLayout(VertexInputLayout inputLayout)
        {
            currentVertexInputLayout = inputLayout;
            SetupInputLayout();
        }

        public void SetVertexShaderConstantBuffer(int slot, Buffer constantBuffer)
        {
            VertexShader.SetConstantBuffer(slot, constantBuffer);
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="x">The x coordinate of the viewport.</param>
        /// <param name="y">The y coordinate of the viewport.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="minZ">The min Z.</param>
        /// <param name="maxZ">The max Z.</param>
        /// <remarks>
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>
        /// </remarks>
        /// <msdn-id>ff476480</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>
        public void SetViewport(float x, float y, float width, float height, float minZ = 0.0f, float maxZ = 1.0f)
        {
            viewports[0] = new ViewportF(x, y, width, height, minZ, maxZ);
            RasterizerStage.SetViewport(x, y, width, height, minZ, maxZ);
        }

        /// <summary>
        /// Binds a single viewport to the rasterizer stage.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <remarks>
        /// <p></p><p>All viewports must be set atomically as one operation. Any viewports not defined by the call are disabled.</p><p>Which viewport to use is determined by the SV_ViewportArrayIndex semantic output by a geometry shader; if a geometry shader does not specify the semantic, Direct3D will use the first viewport in the array.</p>
        /// </remarks>
        /// <msdn-id>ff476480</msdn-id>
        /// <unmanaged>void ID3D11DeviceContext::RSSetViewports([In] unsigned int NumViewports,[In, Buffer, Optional] const void* pViewports)</unmanaged>
        /// <unmanaged-short>ID3D11DeviceContext::RSSetViewports</unmanaged-short>
        public void SetViewport(ViewportF viewport)
        {
            viewports[0] = viewport;
            RasterizerStage.SetViewport(viewport);
        }

        internal void SetCurrentEffect(Technique technique)
        {
            Contract.Requires<ArgumentNullException>(technique != null, "technique");
            
            SetShader((Graphics.Shaders.VertexShader) technique[ShaderType.Vertex]);
            if (technique.ContainsShader(ShaderType.Pixel))
                SetShader((Graphics.Shaders.PixelShader) technique[ShaderType.Pixel]);
            currentTechnique = technique;
        }
        #region IDirect3DProvider


        #endregion IDirect3DProvider
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                if (Presenter != null)
                {
                    // Invalid for WinRT - throwing a "Value does not fall within the expected range" Exception
#if !WIN8METRO
    // Make sure that the Presenter is reverted to window before shutting down
    // otherwise the Direct3D11.Device will generate an exception on Dispose()
                    Presenter.IsFullScreen = false;
#endif
                    Presenter.Dispose();
                    Presenter = null;
                    GraphicsAdapter.Dispose();
                    WICHelper.Dispose();
                }

            }

            base.Dispose(disposeManagedResources);
        }

        private void CommonSetRenderTargets(RenderTargetView rtv)
        {
            currentRenderTargetViews[0] = rtv;
            for (int i = 1; i < actualRenderTargetViewCount; i++)
                currentRenderTargetViews[i] = null;
            actualRenderTargetViewCount = 1;
            currentRenderTargetView = rtv;

            // Setup the viewport from the rendertarget view
            TextureView textureView;
            if (AutoViewportFromRenderTargets && rtv != null && (textureView = rtv.Tag as TextureView) != null)
            {
                SetViewport(new ViewportF(0, 0, textureView.Width, textureView.Height));
            }
        }

        private void CommonSetRenderTargets(RenderTargetView[] rtvs)
        {
            var rtv0 = rtvs.Length > 0 ? rtvs[0] : null;
            for (int i = 0; i < rtvs.Length; i++)
                currentRenderTargetViews[i] = rtvs[i];
            for (int i = rtvs.Length; i < actualRenderTargetViewCount; i++)
                currentRenderTargetViews[i] = null;
            actualRenderTargetViewCount = rtvs.Length;
            currentRenderTargetView = rtv0;

            // Setup the viewport from the rendertarget view
            TextureView textureView;
            if (AutoViewportFromRenderTargets && rtv0 != null && (textureView = rtv0.Tag as TextureView) != null)
            {
                SetViewport(new ViewportF(0, 0, textureView.Width, textureView.Height));
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            // Default null VertexBuffers used to reset
            if (ResetSlotsPointers == IntPtr.Zero)
            {
                // CommonShaderStage.InputResourceSlotCount is the maximum of resources bindable in the whole pipeline
                ResetSlotsPointers = ToDispose(SharpDX.Utilities.AllocateClearedMemory(SharpDX.Utilities.SizeOf<IntPtr>() *
                                                                                       CommonShaderStage.InputResourceSlotCount));
            }

            InputAssembler = context.InputAssembler;
            VertexShader = context.VertexShader;
            //DomainShader = context.DomainShader;
            //HullShader = context.HullShader;
            //GeometryShader = context.GeometryShader;
            RasterizerStage = context.Rasterizer;
            PixelShader = context.PixelShader;
            OutputMerger = context.OutputMerger;
            //ComputeShader = context.ComputeShader;
            //ShaderStages = new CommonShaderStage[]
            //                   {
            //                       context.VertexShader,
            //                       context.HullShader,
            //                       context.DomainShader,
            //                       context.GeometryShader,
            //                       context.PixelShader,
            //                       context.ComputeShader
            //                   };

            //Performance = new GraphicsPerformance(this);
        }

        private void SetupInputLayout()
        {
            if (CurrentTechnique == null)
                throw new InvalidOperationException("Cannot perform a Draw/Dispatch operation without a Technique applied.");

            var inputLayout = CurrentTechnique.InputLayout;
            InputAssembler.InputLayout = inputLayout;
        }

        RenderTargetView ITarget.RenderTarget => currentRenderTargetView;

        DepthStencilView ITarget.DepthStencilBuffer => currentDepthStencilView;
    }
}