using Odyssey.Daedalus.Data;
using Odyssey.Engine;
using Odyssey.Graphics.Effects;

namespace Odyssey.Daedalus.Shaders
{
    public static class Float
    {
        public static Vector ShadowBias
        {
            get { return new Vector { Name = Param.Floats.ShadowBias, Type = Type.Float, EngineReference = ReferenceFactory.Effect.SpriteSize }; }
        }

        public static Vector GlowStrength
        {
            get { return new Vector { Name = Param.Floats.GlowStrength, EngineReference = ReferenceFactory.Effect.GlowStrength, Type = Type.Float }; }
        }



        #region Bloom
        public static Vector BloomThreshold
        {
            get
            {
                return new Vector()
                {
                    Name = Param.Floats.BloomThreshold,
                    EngineReference = ReferenceFactory.Effect.BloomThreshold,
                    Type = Type.Float
                };
            }
        }

        #endregion

    }
}
