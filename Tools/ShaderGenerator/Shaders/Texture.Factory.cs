using Odyssey.Engine;
using Odyssey.Graphics.Materials;
using ShaderGenerator.Data;

namespace Odyssey.Tools.ShaderGenerator.Shaders
{
    public partial class Texture : Variable
    {
        internal const string Key = "Key";
        internal const string SamplerIndex = "SamplerIndex";
        internal const string Procedural = "Procedural";
        internal const string UpdateFrequency = "UpdateFrequency";

        public static Texture Diffuse
        {
            get
            {
                return new Texture
                {
                    Name = Param.Textures.DiffuseMap,
                    Type = Shaders.Type.Texture2D,
                    Index = 0,
                    ShaderReference = new ShaderReference(TextureReference.Diffuse)
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
                    ShaderReference = new ShaderReference(TextureReference.ShadowMap)
                };
            }
        }
    }
}
