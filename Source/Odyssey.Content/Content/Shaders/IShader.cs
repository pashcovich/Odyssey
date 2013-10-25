using Odyssey.Graphics.Materials;
using System.Collections.Generic;

namespace Odyssey.Content.Shaders
{
    public interface IShaderFeature
    {
        FeatureType Type { get; }
        object Value { get; }
    }

    public interface IShaderReference
    {
        int Index { get; set; }
        ReferenceType Type { get; }
        object Value { get; }
    }

    public interface IConstantBufferReference
    {
        int Index { get; }
        UpdateFrequency UpdateFrequency { get; }
        IEnumerable<IShaderReference> References { get; }
    }
}
