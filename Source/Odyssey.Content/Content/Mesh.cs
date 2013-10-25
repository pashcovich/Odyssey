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
    public class Mesh : Component, IByteSize
    {
        public VertexFormat VertexFormat { get; private set; }
        public Format IndexFormat { get; private set; }
        public Buffer VertexBuffer { get; private set; }
        public Buffer IndexBuffer { get; private set; }
        public int VertexCount { get; private set; }
        public int VertexSize { get; private set; }

        public Mesh(VertexFormat vertexFormat, Buffer vertexBuffer, Format indexFormat, Buffer indexBuffer)
        {
            VertexFormat = vertexFormat;
            IndexFormat = indexFormat;
            VertexBuffer = vertexBuffer;
            IndexBuffer = indexBuffer;
        }


        int IByteSize.ByteSize
        {
            get { return VertexBuffer.Description.SizeInBytes + IndexBuffer.Description.SizeInBytes; }
        }
    }
}
