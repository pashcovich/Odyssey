using System;

namespace Odyssey.Graphics.Materials
{
    [Flags]
    public enum VertexShaderFlags
    {
        None = 0,
        Position = 1,
        Normal = 2,
        TextureUV = 4,
        Shadows = 1024,
        Instancing = 2048,
    }

    [Flags]
    public enum PixelShaderFlags
    {
        None = 0,
        Diffuse = 1,
        DiffuseMap = 2,
        Specular = 4,
        SpecularMap = 8,
        Shadows = 16,
        ShadowMap = 32
    }

    public enum ShaderModel
    {
        SM20_level_9_1,
        SM20_level_9_3,
        SM30,
        SM40,
        SM41,
        SM50,
    }

    public enum FeatureLevel
    {
        PS_4_0_Level_9_1,
        PS_4_0_Level_9_3,
        PS_5_0,
        VS_4_0_Level_9_1,
        VS_4_0_Level_9_3,
    }

    public enum FeatureType
    {
        None,
        VertexShader,
        PixelShader,
        Shadows,
        TextureMaps
    }

    public enum UpdateFrequency
    {
        Static,
        PerFrame,
        PerInstance,
    }


}
