using Odyssey.Engine;

namespace Odyssey.Daedalus.Shaders
{
    public partial class Sampler
    {
        internal const string Filter = "Filter";
        internal const string TextureAddressMode = "TextureAddressMode";
        internal const string Comparison = "Comparison";
        internal const string SamplerName = "Name";

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
                Sampler sampler = new Sampler { Name = Param.Samplers.MinMagMipLinearWrap, Type = Type.Sampler};
                sampler.SetMarkup(SamplerName, "LinearClamp");
                sampler.SetMarkup(Filter, SharpDX.Direct3D11.Filter.MinMagMipLinear);
                sampler.SetMarkup(TextureAddressMode, SharpDX.Direct3D11.TextureAddressMode.Clamp);
                sampler.SetMarkup(Comparison, SharpDX.Direct3D11.Comparison.Never);
                return sampler;
            }
        }
    }
}
