using Odyssey.Graphics.Materials;
using System.Collections.Generic;
namespace Odyssey.Content.Shaders
{
    public interface IShaderObject
    {
        byte[] ByteCode { get; }
        FeatureLevel FeatureLevel { get; }
        string Name { get; }
        IEnumerable<IConstantBufferReference> ShaderReferences { get; }
        IEnumerable<IShaderFeature> ShaderFeatures { get; }
    }
}
