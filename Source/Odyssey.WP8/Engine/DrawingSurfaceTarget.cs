using Odyssey.Engine;
using SharpDX;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.WP8.Engine
{
    public class DrawingSurfaceTarget : Component, IDirectXTarget, IDirect3DProvider, IDirectXProvider
    {
        private Windows.Foundation.Size renderTargetSize;
        private Windows.Foundation.Rect windowBounds;

        // Direct3D Objects.
        private Device device;
        private DeviceContext context;
        private Texture2D renderTarget;
        private RenderTargetView renderTargetView;
        private DepthStencilView depthStencilView;
        private WP8DeviceSettings settings;

        public event EventHandler<InitializeDirectXEventArgs> Initialize;

        public DrawingSurfaceTarget(WP8DeviceSettings settings)
        {
            this.settings = settings;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Update(Device device, DeviceContext context, RenderTargetView renderTargetView)
        {
            if (this.device != device)
            {
                this.device = device;
                OnInitialize(new InitializeDirectXEventArgs(this, settings));
            }

            this.context = context;
            this.renderTargetView = renderTargetView;

            CreateWindowSizeDependentResources();

        }

        protected virtual void OnInitialize(InitializeDirectXEventArgs e)
        {
            var handler = Initialize;
            if (handler != null)
                handler(this, e);
        }

        public virtual void CreateWindowSizeDependentResources()
        {
            var resource = renderTargetView.Resource;
            using (var texture2D = new Texture2D(resource.NativePointer))
            {

                var currentWidth = (int)renderTargetSize.Width;
                var currentHeight = (int)renderTargetSize.Height;

                if (currentWidth != texture2D.Description.Width &&
                    currentHeight != texture2D.Description.Height)
                {
                    renderTargetSize.Width = texture2D.Description.Width;
                    renderTargetSize.Height = texture2D.Description.Height;

                    Utilities.Dispose(ref depthStencilView);

                    using (var depthTexture = new Texture2D(device, new Texture2DDescription()
                        {
                            Width = (int)renderTargetSize.Width,
                            Height = (int)renderTargetSize.Height,
                            ArraySize = 1,
                            BindFlags = BindFlags.DepthStencil,
                            CpuAccessFlags = CpuAccessFlags.None,
                            Format = SharpDX.DXGI.Format.D24_UNorm_S8_UInt,
                            MipLevels = 1,
                            OptionFlags = ResourceOptionFlags.None,
                            SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                            Usage = ResourceUsage.Default
                        }))
                        depthStencilView = new DepthStencilView(device, depthTexture);
                }
            }

            windowBounds.Width = renderTargetSize.Width;
            windowBounds.Height = renderTargetSize.Height;

            // Create a viewport descriptor of the full window size.
            var viewport = new SharpDX.ViewportF(0, 0, (float)renderTargetSize.Width, (float)renderTargetSize.Height);

            context.Rasterizer.SetViewport(viewport);
        }

        public virtual void UpdateForWindowSizeChange(float width, float height)
        {
            renderTargetSize.Width = width;
            renderTargetSize.Height = height;

            RenderTargetView[] nullViews = { null };
            renderTarget = null;
            renderTargetView = null;
            depthStencilView = null;
            context.Flush();

            CreateWindowSizeDependentResources();
        }


        public Texture2D BackBuffer
        {
            get { return renderTarget; }
        }

        public DepthStencilView DepthStencilView
        {
            get { return depthStencilView; }
        }

        public RenderTargetView RenderTargetView
        {
            get { return renderTargetView; }
        }

        public IDirect3DProvider Direct3D
        {
            get { return this; }
        }

        public Device Device
        {
            get { return device; }
        }

        public DeviceContext Context
        {
            get { return context; }
        }
    }
}
