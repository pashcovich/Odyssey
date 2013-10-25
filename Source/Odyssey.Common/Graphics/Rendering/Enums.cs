using System;

namespace Odyssey.Graphics.Rendering
{
    public enum ResourceType
    {
        None,
        Texture1D,
        Texture2D,
    }

    public enum CommandType
    {
        Render,
        Update,
        ComputeShadows,
        RasterizerStateChange,
        BlendStateChange,
        DepthStencilStateChange,
        UserInterfaceRenderCommand,
        Action
    }

    public enum RenderingOrderType
    {
        OpaqueGeometry,
        AdditiveBlendingGeometry,
        SubtractiveBlendingGeometry,
        First,
        Last
    }

    public enum ShaderType
    {
        Vertex,
        Pixel,
    }


    public enum CommandAttributes
    {
        Default,
        PreProcess,
        PostProcess
    }

    [Flags]
    public enum InstanceSemantic
    {
        None = 0,
        World = 1,
        Color = 2,
    }

}
