using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.WP8.Engine
{
	class OdysseyContentProvider : DrawingSurfaceBackgroundContentProviderNativeBase
	{
		private readonly OdysseyInterop controller;
		DrawingSurfaceRuntimeHost host;
		DrawingSurfaceSynchronizedTexture synchronizedTexture;

		public OdysseyContentProvider(OdysseyInterop controller)
		{
			this.controller = controller;
		}

		public override void Connect(DrawingSurfaceRuntimeHost host, Device device)
		{
			this.host = host;
			controller.Connect();
		}

		public override void Disconnect()
		{
			controller.Disconnect();
			host = null;
			synchronizedTexture = null;
		}

		public override void Draw(Device device, DeviceContext context, RenderTargetView renderTargetView)
		{
			controller.Render(device, context, renderTargetView);
			host.RequestAdditionalFrame();
		}

		public override void PrepareResources(DateTime presentTargetTime, ref SharpDX.Size2F desiredRenderTargetSize)
		{
		}


	}
}
