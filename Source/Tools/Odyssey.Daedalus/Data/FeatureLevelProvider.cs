using System;
using System.Collections.Generic;
using System.Linq;
using Odyssey.Graphics.Effects;

namespace Odyssey.Tools.ShaderGenerator.Data
{
    public class FeatureLevelProvider
    {
        public IEnumerable<FeatureLevel> GetPixelShaderFeatureLevels()
        {
            return ((FeatureLevel[])Enum.GetValues(typeof(FeatureLevel))).Where(f => f.ToString().StartsWith("PS"));
        }

        public IEnumerable<FeatureLevel> GetVertexShaderFeatureLevels()
        {
            return ((FeatureLevel[])Enum.GetValues(typeof(FeatureLevel))).Where(f => f.ToString().StartsWith("VS"));
        }
    }
}
