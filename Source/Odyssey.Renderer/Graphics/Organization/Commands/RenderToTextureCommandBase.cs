using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Odyssey.Engine;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

namespace Odyssey.Graphics.Organization.Commands
{
    
    public abstract class RenderToTextureCommandBase : Command, IRenderCommand, IPostProcessCommand
    {
        private readonly Texture2DDescription textureDescription;
        private RenderTarget2D renderTarget;
        private DepthStencilBuffer depthStencil;
        private readonly ViewportF viewport;
        private readonly TargetOutput targetOutput;
        
        protected RenderTarget2D RenderTarget { get { return renderTarget; } }
        protected DepthStencilBuffer DepthStencil { get { return depthStencil; } }
        protected ViewportF Viewport { get { return viewport; } }

        public List<Texture> Inputs { get; protected set; } 
        public Texture Output { get; set; }
      

        protected RenderToTextureCommandBase(IServiceRegistry services, Texture2DDescription textureDescription = default(Texture2DDescription), 
            TargetOutput targetOutput = TargetOutput.NewRenderTarget)
            : base(services, CommandType.PostProcessing)
        {
            
            IDirectXDeviceSettings deviceSettings = services.GetService<IDirectXDeviceSettings>();
            viewport = new Viewport(0, 0, deviceSettings.PreferredBackBufferWidth, deviceSettings.PreferredBackBufferHeight);
            this.targetOutput = targetOutput;

            if (targetOutput == TargetOutput.NewRenderTarget)
            {
                if (textureDescription.Width == 0 || textureDescription.Height == 0)
                    this.textureDescription = new Texture2DDescription()
                    {
                        ArraySize = 1,
                        MipLevels = 1,
                        BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                        Format = deviceSettings.PreferredBackBufferFormat,
                        Width = deviceSettings.PreferredBackBufferWidth,
                        Height = deviceSettings.PreferredBackBufferHeight,
                        SampleDescription = new SampleDescription(1, 0),
                        CpuAccessFlags = CpuAccessFlags.None,
                        OptionFlags = ResourceOptionFlags.None,
                        Usage = ResourceUsage.Default,
                    };
                else
                    this.textureDescription = textureDescription;
            }


        }

        public void SetInputs(IEnumerable<Texture> textures)
        {
            Contract.Requires<ArgumentNullException>(textures != null, "textures");
            Contract.Requires<ArgumentException>(textures.Any());
            Inputs.AddRange(textures);
        }

        public override void Initialize()
        {
            var device = DeviceService.DirectXDevice;
            if (targetOutput == TargetOutput.NewRenderTarget)
            {
                renderTarget = ToDispose(RenderTarget2D.New(device, textureDescription));
                renderTarget.DebugName = string.Format("RT2D_{0}", Name);
                var depthStencilDesc = new Texture2DDescription()
                {
                    ArraySize = 1,
                    MipLevels = 1,
                    BindFlags = BindFlags.DepthStencil,
                    Format = device.DepthStencilBuffer.Format,
                    Width = textureDescription.Width,
                    Height = textureDescription.Height,
                    SampleDescription = textureDescription.SampleDescription,
                    CpuAccessFlags = CpuAccessFlags.None,
                    Usage = ResourceUsage.Default,
                    OptionFlags = ResourceOptionFlags.None
                };

                depthStencil = ToDispose(DepthStencilBuffer.New(device, depthStencilDesc));
            }

            Output = renderTarget;
        }

        public override void Unload()
        {
            if (renderTarget != null)
                renderTarget.Dispose();

            if (depthStencil != null)
                depthStencil.Dispose();

            if (Output != null)
                Output.Dispose();
        }

        public override void Execute()
        {
            PreRender();
            Render();
            PostRender();
        }

        public virtual void PreRender()
        {
            var device = DeviceService.DirectXDevice;
            if (targetOutput == TargetOutput.BackBuffer)
                device.SetRenderTargets(device.DepthStencilBuffer, device.RenderTarget);
            else
            {
                device.SetRenderTargets(DepthStencil, RenderTarget);
                device.Clear(Color.Transparent);
            }
            device.SetPixelShaderConstantBuffer(0, null);
            

        }

        public abstract void Render();

        public virtual void PostRender()
        {
            var device = DeviceService.DirectXDevice;
            //device.SetRenderTargets(device.DepthStencilBuffer, device.RenderTarget);
        }


    }
}
