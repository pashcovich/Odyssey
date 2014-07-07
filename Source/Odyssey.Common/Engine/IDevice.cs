#region Using Directives

using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using Device = SharpDX.Direct2D1.Device;
using Device1 = SharpDX.Direct3D11.Device1;
using DeviceContext = SharpDX.Direct2D1.DeviceContext;
using DeviceContext1 = SharpDX.Direct3D11.DeviceContext1;
using Factory = SharpDX.DirectWrite.Factory;

#endregion Using Directives

namespace Odyssey.Engine
{
#if !WP8

    public interface IDirect2DProvider
    {
        Device Device { get; }

        DeviceContext Context { get; }

        Factory1 Factory { get; }
    }

    public interface IDirectWriteProvider
    {
        Factory Factory { get; }
    }

#endif

    public interface IDirect3DProvider
    {
        bool IsDebugMode { get; }

#if DIRECTX11_1

        Device1 Device { get; }

        DeviceContext1 Context { get; }

#else
        SharpDX.Direct3D11.Device Device { get; }
        SharpDX.Direct3D11.DeviceContext Context { get; }
#endif
    }
}