namespace Odyssey.Tools.ShaderGenerator.Shaders.Methods
{
    public abstract partial class MethodBase
    {
        internal struct Floats
        {
            internal const string ShadowBias = "fBias";
            internal const string ShadowFactor = "fShadowFactor";
            internal const string SpriteSize = "fSpriteSize";
            internal const string tU = "tU";
            internal const string tV = "tV";
            internal const string tW = "tW";
            internal const string Saturation = "fSaturation";
        }

        internal struct Methods
        {
            internal const string PCFShadows = "PCFShadows";
            internal const string TexOffset = "TexOffset";
        }

        internal struct Colors
        {
            internal const string Color = "color";
            internal const string Ambient = "cAmbient";
            internal const string Diffuse = "cDiffuse";
            internal const string DiffuseMap = "cDiffuseMap";
            internal const string NormalMap = "cNormalMap";
            internal const string Specular = "cSpecular";
        }

        internal struct Samplers
        {
            internal const string Linear = "sLinear";
            internal const string ShadowMap = "sShadowMap";
        }

        internal struct Structs
        {
            internal const string Light = "light";
            internal const string Material = "material";
        }

        internal struct Textures
        {
            internal const string DiffuseMap = "tDiffuseMap";
            internal const string ShadowMap = "tShadowMap";
            internal const string NormalMap = "tNormalMap";
        }

        internal struct Vectors
        {
            internal const string Position = "vPosition";
            internal const string PositionInstance = "pInstance";
            internal const string ScreenSize = "vScreenSize";
            internal const string DiffuseMapCoordinates = "vDiffuseMapCoordinates";
            internal const string NormalMapCoordinates = "vNormalMapCoordinates";
            internal const string LightDirection = "vLightDirection";
            internal const string Normal = "vNormal";
            internal const string NormalMap = "vNormalMap";
            internal const string Tangent= "vTangent";
            internal const string Bitangent = "vBitangent";
            internal const string ShadowProjection = "vShadowProjection";
            internal const string ViewDirection = "vViewDirection";
        }

        internal struct Matrices
        {
            internal const string TangentSpace = "mTangentSpace";
        }
    }
}