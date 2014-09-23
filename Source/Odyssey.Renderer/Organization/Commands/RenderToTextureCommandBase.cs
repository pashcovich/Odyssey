using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Organization;
using Odyssey.Graphics.PostProcessing;
using Odyssey.Graphics.Shaders;
using SharpDX;
using SharpDX.Direct3D11;
using TextureDescription = Odyssey.Graphics.TextureDescription;

namespace Odyssey.Organization.Commands
{
	public abstract class RenderToTextureCommandBase : EngineCommand, IRenderCommand, IPostProcessCommand, ITarget
	{
		private readonly Texture2DDescription textureDescription;
		private RenderTarget2D renderTarget;
		private DepthStencilBuffer depthStencil;
		private readonly ViewportF viewport;
		private readonly OutputRule outputRule;
		private readonly List<Texture> inputs;

	    protected RenderTarget2D RenderTarget { get { return renderTarget; } }
		protected DepthStencilBuffer DepthStencil { get { return depthStencil; } }
		protected ViewportF Viewport { get { return viewport; } }

		protected TextureDescription Description { get { return textureDescription; } }

		public IEnumerable<Texture> Inputs { get { return inputs; } } 
		public Texture Output { get; set; }

		protected RenderToTextureCommandBase(IServiceRegistry services, Texture2DDescription textureDescription = default(Texture2DDescription), 
			OutputRule outputRule = OutputRule.NewRenderTarget)
			: base(services, CommandType.PostProcessing)
		{
			
			IDirectXDeviceSettings deviceSettings = services.GetService<IDirectXDeviceSettings>();
			viewport = new Viewport(0, 0, deviceSettings.PreferredBackBufferWidth, deviceSettings.PreferredBackBufferHeight);
			this.outputRule = outputRule;
			inputs = new List<Texture>();

			if (outputRule == OutputRule.NewRenderTarget)
			{
				if (textureDescription.Width == 0 || textureDescription.Height == 0)
					this.textureDescription = PostProcessor.GetTextureDescription(deviceSettings.PreferredBackBufferWidth, deviceSettings.PreferredBackBufferHeight);
				else
					this.textureDescription = textureDescription;
			}
		}

		public void SetInputs(IEnumerable<Texture> textures)
		{
			inputs.AddRange(textures);
		}
		public override void Initialize()
		{
			var device = DeviceService.DirectXDevice;
			if (outputRule == OutputRule.NewRenderTarget)
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

		public override void Execute()
		{
			PreRender();
			Render();
			PostRender();
		}

		public virtual void PreRender()
		{
			var device = DeviceService.DirectXDevice;
			if (outputRule != OutputRule.Output)
			{
				device.SetRenderTargets(DepthStencil, RenderTarget);
				device.Clear(Color.Black);
				
			}
			device.SetPixelShaderConstantBuffer(0, null);
		}

		public abstract void Render();

		public virtual void PostRender()
		{
		}

		#region ITarget
		DepthStencilView ITarget.DepthStencilBuffer
		{
			get { return DepthStencil; }
		}

		RenderTargetView ITarget.RenderTarget
		{
			get { return RenderTarget; }
		} 
		#endregion
	}
}
