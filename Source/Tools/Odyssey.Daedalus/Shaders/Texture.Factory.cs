using Odyssey.Daedalus.Data;
using Odyssey.Engine;
using Odyssey.Graphics;
using Odyssey.Graphics.Effects;

namespace Odyssey.Daedalus.Shaders
{
    public partial class Texture : Variable
    {
        internal const string Key = "Key";
        internal const string SamplerIndex = "SamplerIndex";
        internal const string Procedural = "Procedural";
        internal const string UpdateType = "UpdateType";

        public static Texture Diffuse
        {
            get
            {
                return new Texture
                {
                    Name = Param.Textures.DiffuseMap,
                    Type = Shaders.Type.Texture2D,
                    Index = 0,
                    EngineReference = ReferenceFactory.Texture.Diffuse
                };
            }
        }

        public static Texture CubeMap
        {
            get
            {
                return new Texture
                {
                    Name = Param.Textures.CubeMap,
                    Type = Shaders.Type.TextureCube,
                    Index = 0,
                    EngineReference = ReferenceFactory.Texture.Diffuse
                };
            }
        }

        public static Texture NormalMap
        {
            get
            {
                return new Texture
                {
                    Name = Param.Textures.NormalMap,
                    Type = Type.Texture2D,
                    Index = 0,
                    EngineReference = ReferenceFactory.Texture.Normal
                };
            }
        }

        public static Texture ShadowMap
        {
            get
            {
                return new Texture
                {
                    Name = Param.Textures.ShadowMap,
                    Type = Shaders.Type.Texture2D,
                    Index = 0,
                    EngineReference = ReferenceFactory.Texture.Shadow
                };
            }
        }
    }
}
