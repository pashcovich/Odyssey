using System;

namespace Odyssey.Graphics.Effects
{
    public enum ShaderType
    {
        None,
        Vertex ,
        Pixel 
    }

    public enum FeatureLevel
    {
        PS_2_0,
        PS_2_A,
        PS_2_B,
        PS_3_0,
        PS_4_0_Level_9_1,
        PS_4_0_Level_9_3,
        PS_4_0,
        PS_4_1,
        PS_5_0,
        VS_2_0,
        VS_2_A,
        VS_3_0,
        VS_4_0_Level_9_1,
        VS_4_0_Level_9_3,
        VS_4_0,
        VS_4_1,
        VS_5_0,
    }

    [Flags]
    public enum PixelShaderFlags
    {
        None = 0,
        Diffuse = 1 << 0,
        DiffuseMap = 1 << 1,
        Specular = 1 << 2,
        SpecularMap = 1 << 3,
        Shadows = 1 << 4,
        ShadowMap = 1 << 5,
        CubeMap = 1 << 6,
        NormalMap = 1 << 7,
        All = ~0,
    }

    public enum ShaderConfiguration
    {
        Debug,
        Release
    }

    public enum ShaderModel
    {
        Any,
        SM_2_0,
        SM_2_A,
        SM_2_B,
        SM_3_0,
        SM_4_0_Level_9_1,
        SM_4_0_Level_9_3,
        SM_4_0,
        SM_4_1,
        SM_5_0,
    }

    public enum UpdateType
    {
        None,
        SceneStatic,
        SceneFrame,
        InstanceStatic,
        InstanceFrame,
    }

    [Flags]
    public enum VertexShaderFlags
    {
        None = 0,
        Position = 1 << 0,
        Normal = 1 << 1,
        TextureUV = 1 << 2,
        TextureUVW = 1 << 3,
        Color = 1 << 4,
        Tangent = 1 << 5,
        Barycentric = 1 << 6,
        ShadowProjection = 1 << 7,
        InstanceWorld = 1 << 8,

        All = ~0,
    }

}