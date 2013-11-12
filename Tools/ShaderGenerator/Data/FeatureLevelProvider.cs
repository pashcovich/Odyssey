using Odyssey.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //return new[] { FeatureLevel.VS_4_0_Level_9_1,
            //    FeatureLevel.VS_4_0_Level_9_3, 
            //    FeatureLevel.VS_5_0};
        }
    }
}
