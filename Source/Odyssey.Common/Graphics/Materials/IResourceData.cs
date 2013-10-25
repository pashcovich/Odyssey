using Odyssey.Engine;
using SharpDX.Direct3D11;
using System;
namespace Odyssey.Graphics.Materials
{
    public interface IResourceData
    {
        string Key { get; set; }
        bool IsEmpty { get; }
        TextureReference Reference { get; }
        Resource Resource { get; }
        ShaderResourceView ResourceView { get; }
        SamplerState Sampler { get; }
        int SamplerSlot { get; }
        int TextureSlot { get; }

        void Initialize(IDirectXProvider directX);
    }
}
