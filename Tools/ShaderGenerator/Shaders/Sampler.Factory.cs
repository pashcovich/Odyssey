using Odyssey.Engine;
using Odyssey.Tools.ShaderGenerator.Shaders.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public partial class Sampler
    {
        internal const string Filter = "Filter";
        internal const string TextureAddressMode = "TextureAddressMode";
        internal const string Comparison = "Comparison";

                //    SamplerStateDescription sLinearCmpZero = SamplerFactory.CreateDescription(filter:Filter.ComparisonMinMagMipLinear, 
                //addressMode: TextureAddressMode.Mirror, function: Comparison.LessEqual);
        public static Sampler MinMagMiLinearMirrorLessEqual
        {
            get
            {
                Sampler sampler = new Sampler { Name = Param.Samplers.Generic, Type = Shaders.Type.SamplerComparisonState};
                sampler.SetMarkup(Filter, SharpDX.Direct3D11.Filter.MinMagMipLinear);
                sampler.SetMarkup(TextureAddressMode, SharpDX.Direct3D11.TextureAddressMode.Mirror);
                sampler.SetMarkup(Comparison, SharpDX.Direct3D11.Comparison.LessEqual);
                return sampler;
            }
        }

        public static Sampler MinMagMipLinearWrap
        {
            get
            {
                Sampler sampler = new Sampler { Name = Param.Samplers.MinMagMipLinearWrap, Type = Shaders.Type.Sampler};
                sampler.SetMarkup(Filter, SharpDX.Direct3D11.Filter.MinMagMipLinear);
                sampler.SetMarkup(TextureAddressMode, SharpDX.Direct3D11.TextureAddressMode.Wrap);
                sampler.SetMarkup(Comparison, SharpDX.Direct3D11.Comparison.Never);
                return sampler;
            }
        }
    }
}
