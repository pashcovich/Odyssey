using Odyssey.Engine;
using Odyssey.Geometry;
using Odyssey.Graphics.Meshes;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content
{
    public class TkMeshPart : Component, IByteSize
    {
        public VertexFormat VertexFormat { get; private set; }
        public Format IndexFormat { get; private set; }
        public int VertexCount { get; private set; }
        public int VertexSize { get; private set; }
        public int IndexCount { get; private set; }
        public int IndexSize { get; private set; }
        public Buffer VertexBuffer { get; private set; }
        public Buffer IndexBuffer { get; private set; }


        public TkMeshPart(string name, VertexFormat vertexFormat, int vertexSize, Buffer vertexBuffer, Format indexFormat, Buffer indexBuffer)
        {
            Name = name;
            VertexFormat = vertexFormat;
            IndexFormat = indexFormat;
            VertexBuffer = ToDispose(vertexBuffer);
            IndexBuffer = ToDispose(indexBuffer);
            VertexSize = vertexSize;
            VertexCount = VertexBuffer.Description.SizeInBytes / VertexSize;
            IndexSize = IndexFormat == Format.R32_UInt ? 4 : 2;
            IndexCount = IndexBuffer.Description.SizeInBytes / IndexSize;
        }

        public int ByteSize
        {
            get { return VertexBuffer.Description.SizeInBytes + IndexBuffer.Description.SizeInBytes; }
        }
    }
}
