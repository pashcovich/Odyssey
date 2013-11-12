namespace Odyssey.Engine
{
    internal struct Param
    {
        internal struct Floats
        {
            internal const string SpriteSize = "fSize";
            internal const string ShadowBias = "fBias";
        }

        internal struct ConstantBuffer
        {
            internal const string Material = "cbMaterial";
            internal const string PerFrame = "cbFrame";
            internal const string PerInstance = "cbInstance";
            internal const string Static = "cbStatic";
        }

        internal struct Light
        {
            internal const string Intensity = "Intensity";
            internal const string Radius = "Radius";
            internal const string Diffuse = "Diffuse";
            internal const string Position = "Position";
            internal const string Direction = "Direction";
            internal const string Params = "Params";
        }

        internal struct Material
        {
            internal const string kA = "kA";
            internal const string kD = "kD";
            internal const string kS = "kS";
            internal const string Diffuse = "Diffuse";
            internal const string Ambient = "Ambient";
            internal const string Specular = "Specular";
            internal const string SpecularPower = "SpecularPower";
        }

        internal struct SemanticVariables
        {
            internal const string Position = "Position";
            internal const string ObjectPosition = "Position";
            internal const string WorldPosition = "WorldPosition";
            internal const string Normal = "Normal";
            internal const string Color = "Color";
            internal const string ViewDirection = "ViewDirection";
            internal const string Texture = "Texture";
            internal const string SVTarget = "SV_Target";
            internal const string ShadowProjection = "ShadowProjection";
        }

        internal struct Matrices
        {
            internal const string LightView = "mLightView";
            internal const string LightProjection = "mLightProjection";
            internal const string View = "mView";
            internal const string World = "mWorld";
            internal const string WorldInverse = "mWorldInverse";
            internal const string WorldInverseTranspose = "mWorldInverseTranspose";
            internal const string WorldViewProjection = "mWorldViewProjection";
            internal const string Projection = "mProjection";
        }

        internal struct Samplers
        {
            internal const string MinMagMipLinearWrap = "sLinear";
            internal const string Generic = "sampler";
        }

        internal struct Struct
        {
            internal const string PointLight = "lPoint";
            internal const string Material = "material";
        }

        internal struct Textures
        {
            internal const string CubeMap = "tDiffuseMap";
            internal const string DiffuseMap = "tDiffuseMap";
            internal const string ShadowMap = "tShadowMap";
        }

        internal struct Variables
        {
            internal const string Color = "color";
            internal const string DiffuseColor = "cDiffuse";
            internal const string AmbientColor = "cAmbient";
            internal const string SpecularColor = "cSpecular";
        }

        internal struct Vectors
        {
            internal const string ScreenSize = "vScreenSize";
            internal const string CameraPosition = "vCameraPosition";
            internal const string SpritePosition = "vSpritePosition";
        }
    }
}