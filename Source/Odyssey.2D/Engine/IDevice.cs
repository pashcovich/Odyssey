#if !ODYSSEY_ENGINE && DIRECTX11_1
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Engine
{
#if !WP8
    public interface IDirect2DProvider
    {
        SharpDX.Direct2D1.Device Device { get;  }
        SharpDX.Direct2D1.DeviceContext Context { get; }
        SharpDX.Direct2D1.Factory1 Factory { get; }
    }

    public interface IDirectWriteProvider
    {
        SharpDX.DirectWrite.Factory Factory { get; }
    }

    public interface IDirect3DProvider
    {
        SharpDX.Direct3D11.Device1 Device { get;  }
        SharpDX.Direct3D11.DeviceContext1 Context { get;  }
    }
#else
    public interface IDirect3DProvider
    {
        SharpDX.Direct3D11.Device Device { get; }
        SharpDX.Direct3D11.DeviceContext Context { get; }
    }
#endif


    public interface IDirectXProvider
    {
#if !WP8
        IDirect2DProvider Direct2D { get; }
        IDirectWriteProvider DirectWrite { get; }
#endif
        IDirect3DProvider Direct3D { get; }
    }

    public interface IDeviceDependentComponent
    {
        void Initialize(InitializeDirectXEventArgs e);
    }
}
#endif