using Odyssey.Graphics.Effects;
using Odyssey.Utilities;
using Odyssey.Utilities.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Graphics.Models
{
    public static class ModelAnalyser
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