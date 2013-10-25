using Odyssey.Graphics.Effects;
using SharpDX;
using System;
using System.Collections.Generic;
namespace Odyssey.Graphics.Rendering.Lights
{
    public interface ILight
    {
        Color4 Diffuse { get; }
        float Intensity { get; }
        Matrix ProjectionMatrix { get; }
        float Range { get; }
        LightRenderMode RenderMode { get; }
        LightType Type { get; }
        Matrix ViewMatrix { get; }
        Matrix WorldMatrix { get; }
        IEnumerable<IParameter> ToVertexShaderParameterCollection();
        IEnumerable<IParameter> ToPixelShaderParameterCollection();
    }
}
