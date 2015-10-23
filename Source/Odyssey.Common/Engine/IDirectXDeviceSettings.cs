#region Using Directives

using SharpDX.Direct3D;
using SharpDX.DXGI;

#endregion Using Directives

namespace Odyssey.Engine
{
    public interface IDirectXDeviceSettings
    {
        bool IsFullScreen { get; set; }

        bool IsStereo { get; set; }

        Format PreferredBackBufferFormat { get; set; }

        int PreferredBackBufferHeight { get; set; }

        int PreferredBackBufferWidth { get; set; }

        float HorizontalDpi { get; }

        float VerticalDpi { get; }

        FeatureLevel[] PreferredGraphicsProfile { get; set; }

        int PreferredMultiSampleCount { get; set; }

    }
}