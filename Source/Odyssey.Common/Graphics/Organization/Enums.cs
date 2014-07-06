using System;

namespace Odyssey.Graphics.Organization
{

    public enum CommandType
    {
        Undefined,
        Render,
        RasterizerStateChange,
        BlendStateChange,
        DepthStencilStateChange,
        PostProcessing,
    }

    public enum PreferredRasterizerState
    {
        Solid,
        Wireframe
    }

    public enum PreferredBlendState
    {
        Disabled,
        Additive,
        /// <summary>
        /// SourceBlend = SourceAlpha/1, DestBlend = InvSourceAlpha/1, Op = Add
        /// </summary>
        AlphaBlend 
    }

    public enum PreferredDepthStencilState
    {
        Enabled,
        EnabledComparisonLessEqual,
        DepthWriteDisabled
    }

    public enum RenderingOrderType
    {
        OpaqueGeometry,
        AdditiveBlendingGeometry,
        SubtractiveBlendingGeometry,
        First,
        Last
    }

    [Flags]
    public enum InstanceSemantic
    {
        None = 0,
        World = 1,
        Color = 2,
    }

}
