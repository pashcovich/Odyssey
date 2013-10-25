using Odyssey.Engine;
using Odyssey.Geometry;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;

namespace Odyssey.Graphics.Meshes
{
    public struct VertexDescription
    {
        public VertexFormat Format { get; private set; }
        public int Stride { get; private set; }
        public InputElement[] InputElements { get; private set; }

        public VertexDescription(VertexFormat format, int stride)
            : this()
        {
            Format = format;
            Stride = stride;
            InputElements = GenerateInputElements(format);
        }

        public bool HasSemantic(VertexFormat semantic)
        {
            return (Format & semantic) == semantic;
        }

        private static InputElement CreateInputElement(VertexFormat semantic, int index = 0, int slot = 0)
        {
            int offset = InputElement.AppendAligned;

            switch (semantic)
            {
                case VertexFormat.Position:
                    return new InputElement("POSITION", index, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, slot);

                case VertexFormat.Normal:
                    return new InputElement("NORMAL", index, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, slot);

                case VertexFormat.BiNormal:
                    return new InputElement("BINORMAL", index, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, slot);

                case VertexFormat.Tangent:
                    return new InputElement("TANGENT", index, SharpDX.DXGI.Format.R32G32B32_Float, InputElement.AppendAligned, slot);

                case VertexFormat.TextureUV:
                    return new InputElement("TEXCOORD", index, SharpDX.DXGI.Format.R32G32_Float, InputElement.AppendAligned, slot);

                case VertexFormat.Color4:
                    return new InputElement("COLOR", index, SharpDX.DXGI.Format.R32G32B32A32_Float, offset, slot);

                default:
                    throw Error.InvalidEnum("semantic", typeof(VertexDescription), "CreateInputElement", null, semantic.ToString());
            }
        }

        public static InputElement[] GenerateInputElements(VertexFormat format)
        {
            List<InputElement> elements = new List<InputElement>();

            VertexFormat[] semantics = new VertexFormat[] {
                VertexFormat.Position,
                VertexFormat.Normal,
                VertexFormat.Tangent,
                VertexFormat.BiNormal,
                VertexFormat.TextureUV,
                VertexFormat.Color4,
            };

            foreach (VertexFormat semantic in semantics)
                if ((format & semantic) == semantic)
                    elements.Add(CreateInputElement(semantic));

            return elements.ToArray();
        }
    }
}