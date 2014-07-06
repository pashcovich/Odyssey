using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;

namespace Odyssey.Renderer.Graphics.Materials
{
    public interface IMaterial : IDisposable
    {
        string Name { get; set; }
        Effect Effect { get; }
        Predicate<RasterizerStateDescription> RasterizerCondition { get; }
        Predicate<BlendStateDescription> BlendStateCondition { get; }
        Predicate<DepthStencilStateDescription> DepthStencilCondition { get; }
        RenderableCollectionDescription ItemsDescription { get; }
        TechniqueMapping ActiveTechnique { get; }
        bool GetFeatureStatus(VertexShaderFlags flags);
        bool GetFeatureStatus(PixelShaderFlags flags);
        void Initialize(SceneStateEventArgs args);
        void ApplyInstanceParameters(IInstance instance);
        void ActivateTechnique(TechniqueKey techniqueKey);
        void ActivateTechnique(VertexShaderFlags vsFlags = VertexShaderFlags.None, PixelShaderFlags psFlags = PixelShaderFlags.None);
        IEnumerable<TechniqueKey> FindTechniques(VertexShaderFlags vsFlags = VertexShaderFlags.None, PixelShaderFlags psFlags = PixelShaderFlags.None);
        IEnumerable<IResourceData> Resources { get; }
    }
}