using Odyssey.Engine;
using Odyssey.Geometry;
using Odyssey.Graphics.Materials;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Odyssey.Graphics.Meshes
{
    public interface IGeometry
    {
        string Name { get;}
        int Id { get; }
        bool Inited { get; }
        PrimitiveTopology PrimitiveType { get; }
        Buffer VertexBuffer { get; }
        Buffer IndexBuffer { get;  }
        int IndexCount { get;}
        int VertexCount { get; }
        Format IndexFormat { get; }
        VertexDescription VertexDescription { get;}

        void Dispose();
        void Initialize(IDirectXProvider directX);
        void Render(IDirectXTarget target);
        void Render(IDirectXTarget target, int indexCount, int vertexOffset, int indexOffset, int startIndex, int baseVertex);
        bool CheckFormat(VertexFormat format);

        IPositionVertex[] Vertices { get;}
        ushort[] Indices { get; }
    }


}
