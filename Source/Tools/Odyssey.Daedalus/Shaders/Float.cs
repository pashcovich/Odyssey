using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public static class Float
    {
        public static Vector ShadowBias
        {
            get { return new Vector { Name = Param.Floats.ShadowBias, Type = Type.Float, ShaderReference = new ShaderReference(EngineReference.EntitySpriteSize) }; }
        }

        public static Vector GlowStrength
        {
            get { return new Vector { Name = Param.Floats.GlowStrength, ShaderReference = new ShaderReference(EngineReference.EntityGlowStrength),Type = Type.Float}; }
        }



        #region Bloom
        public static Vector BloomThreshold
        {
            get
            {
                return new Vector()
                {
                    Name = Param.Floats.BloomThreshold,
                    ShaderReference = new ShaderReference(EngineReference.EntityBloomThreshold),
                    Type = Type.Float
                };
            }
        }

        #endregion

    }
}
