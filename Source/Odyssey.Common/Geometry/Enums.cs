using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Geometry
{
    [Flags]
    public enum VertexFormat
    {
        Unknown = 0,
        Position = 1,
        Color4 = 2,
        TextureUV = 4,
        TextureUVW = 8,
        Normal = 16,
        Tangent = 32,
        Bitangent = 64,
        BiNormal = 128,
        Mesh = Position | Normal | TextureUV,
        PositionTextureUV = Position | TextureUV,
        PositionColor4 = Position | Color4,
        TexturedMesh = Position | TextureUV | Normal | Tangent | BiNormal,
    }
}