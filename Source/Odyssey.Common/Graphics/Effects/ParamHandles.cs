namespace Odyssey.Graphics.Effects
{
    public struct ParamHandles
    {
        public struct Color4s
        {
            public const string LightAmbient = "cLightAmbient";
            public const string MaterialDiffuse = "cMaterialDiffuse";
            public const string MaterialAmbient = "cMaterialAmbient";
            public const string MaterialSpecular = "cMaterialSpecular";
            public const string MaterialkA = "kA";
            public const string MaterialkD = "kD";
            public const string MaterialkS = "kS";
            public const string MaterialSpecularPower = "sP";
            public const string MaterialCoefficients = "Coefficients";
        }

        public struct CBuffers
        {
            public const string MaterialBuffer = "mBuffer";
        }

        public struct Floats
        {
            public const string Flags = "flags";
            public const string SpotlightFalloff = "fLightFalloff";

            /// <summary>
            /// Angle of spotlight's inner cone.
            /// </summary>
            public const string SpotlightInnerConeCosine = "fSpotlightInnerConeCosine";

            /// <summary>
            /// Angle of spotlight's outer cone.
            /// </summary>
            public const string SpotlightOuterConeCosine = "fSpotlightOuterConeCosine";

            /// <summary>
            /// Light radius range.
            /// </summary>
            public const string LightRadius = "fLightRadius";

            public const string Scale = "fScale";
            public const string FarClip = "fFarClip";
        }

        public struct Integers
        {
            public const string TextureType = "iTextureType";
        }

        public struct Matrices
        {
            public const string Projection = "mProjection";
            public const string TextureBias = "mTextureBias";
            public const string View = "mView";
            public const string Rotation = "mRotation";
            public const string ViewInverse = "mViewInverse";
            public const string World = "mWorld";
            public const string WorldInverse = "mWorldInverse";
            public const string WorldView = "mWorldView";
            public const string WorldViewInverse = "mWorldViewInverse";
            public const string WorldViewProjection = "mWorldViewProjection";
            public const string LightView = "mLightView";
            public const string LightProjection = "mLightProjection";
        }



        public struct Vectors
        {
            public const string SpotlightDirection = "vSpotlightDirection";
            public const string SpotlightTarget = "vSpotlightTarget";
            public const string EyePosition = "vEyePosition";
            public const string EntityPosition = "vCenter";
        }

        public struct Techniques
        {
            public const string Default = "Default";
        }

        public struct Textures
        {
            public const string Texture1 = "t1";
            public const string Texture2 = "t2";
            public const string Texture3 = "t3";
            public const string DiffuseMap = "tDiffuse";
            public const string NormalMap = "tNormal";
            public const string SpecularMap = "tSpecular";
            public const string ShadowMap = "tShadows";
        }
    }

    public enum SceneVariable
    {
        LightDirection,
        LightPosition,
        LightWorldViewProjection,
        EyePosition,
        FarClip,
        TextureBias,
        CameraView,
        CameraViewInverse,
        CameraWorld,
        CameraWorldInverse,
        CameraWorldViewInverse,
        CameraWorldViewProjection,
        CameraOrthographicProjection,
        CameraProjection,
        CameraViewTranspose,
        CameraProjectionTranspose,
        CameraRotation,
        CameraWorldView,
        CustomFloat,
        CustomInt,
        CustomColor4,
        CustomMatrix,
        CustomTexture2D,
        CustomVector4,
        FloatOp,
        MatrixOp,
        VectorOp,
    }

    public enum MaterialVariable
    {
        MaterialBuffer,
    }

    public enum InstanceVariable
    {
        ObjectPosition,
        InstanceWorld,
        InstanceWorldInverse,
        InstanceWorldInverseTranspose,
        DiffuseMap,
        Diffuse,
        SpriteSize,
        Debug,
    }
}