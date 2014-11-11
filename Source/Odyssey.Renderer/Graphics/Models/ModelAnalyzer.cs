using Odyssey.Graphics.Effects;

namespace Odyssey.Graphics.Models
{
    public static class ModelAnalyzer
    {
        private const VertexShaderFlags vsDefault = VertexShaderFlags.Position | VertexShaderFlags.Normal | VertexShaderFlags.TextureUV;
        private const VertexShaderFlags vsBarycentric = VertexShaderFlags.Position | VertexShaderFlags.Normal | VertexShaderFlags.TextureUV | VertexShaderFlags.Barycentric;

        public static ModelOperation DetectModelRequirements(VertexShaderFlags flags)
        {
            switch (flags)
            {
                case vsBarycentric:
                    return ModelOperation.CalculateBarycentricCoordinatesAndExcludeEdges;

                case vsDefault:
                default:
                    return ModelOperation.None;
            }
        }
    }
}