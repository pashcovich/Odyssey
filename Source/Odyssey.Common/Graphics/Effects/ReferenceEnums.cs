namespace Odyssey.Graphics.Effects
{
    public enum ReferenceType
    {
        None,
        Application,
        Entity,
        Camera,
        Light,
        Material,
        Texture
    }

    public enum EngineReference
    {
        None,
        ApplicationCurrentViewportSize,
        CameraMatrixView,
        CameraMatrixProjection,
        CameraVectorPosition,
        LightDirection,
        LightPointVS,
        LightPointPS,
        LightPointMatrixProjection,
        Material,
        MaterialDiffuse,
        MatrixLightView,
        MatrixLightProjection,
        EntityBlurOffsetsWeights,
        EntityBloomParameters,
        EntityBloomThreshold,
        EntityGlowStrength,
        EntityMatrixWorld,
        EntityMatrixWorldInverse,
        EntityMatrixWorldInverseTranspose,
        EntitySpriteSize,
        EntitySpritePosition,
        
    }

    public enum TextureReference
    {
        None,
        Diffuse,
        ShadowMap,
        NormalMap,
        SpecularMap,
        Procedural,
    }
}
