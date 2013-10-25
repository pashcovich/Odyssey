using Odyssey.Graphics.Effects;
using Odyssey.Graphics.Materials;
using Odyssey.Graphics.Rendering;
using System;
namespace Odyssey.Graphics
{
    public interface IConstantBuffer
    {
        void AddParameter(IParameter parameter);
        void Assemble();
        SharpDX.Direct3D11.Buffer Buffer { get; }
        ShaderType ShaderType { get; }
        int Index { get; set; }
        UpdateFrequency Type { get; }
        void Update();

    }
}
