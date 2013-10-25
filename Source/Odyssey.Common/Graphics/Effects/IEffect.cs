using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using System.Collections.Generic;

namespace Odyssey.Graphics.Effects
{
    public interface IEffect
    {
        void AssembleBuffers();
        SharpDX.Direct3D11.PixelShader PixelShader { get; }
        byte[] PSByteCode { get; }
        string Name { get; }
        IEnumerable<Odyssey.Graphics.IConstantBuffer> SelectBuffers(UpdateFrequency type, ShaderType shaderType);
        void SetConstantBuffer(IConstantBuffer buffer);
        void UpdateBuffers(UpdateFrequency cbType, Odyssey.Graphics.Rendering.ShaderType shaderType);
        SharpDX.Direct3D11.VertexShader VertexShader { get; }
        byte[] VSByteCode { get; }
    }
}
