using System;

namespace Odyssey.Graphics.Meshes
{
    [Flags]
    public enum VertexFormat
    {
        Unknown = 0,
        Position = 1 << 0,
        Color4 = 1 << 1,
        TextureUV = 1 << 2,
        TextureUVW = 1 << 3,
        Normal = 1 << 4,
        Tangent = 1 << 5,
        Bitangent = 1 << 6,
        BiNormal = 1 << 7,
        InstanceWorld = 1 << 8,
    }
}