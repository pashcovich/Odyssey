using Odyssey.Engine;
using Odyssey.Graphics.Rendering2D;
using Odyssey.Graphics.Rendering2D.Shapes;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace Odyssey.Graphics
{
    public class MeshInterface : IRenderable
    {
        const int DefaultSize = 2048;
        
        VertexPositionColor[] vertices;
        ushort[] indices;
        private readonly int maxVertices;
        private readonly int maxIndices;
        Buffer vertexBuffer;
        Buffer indexBuffer;
        private readonly Format format;
        VertexBufferBinding vbBinding;
        int indexCount;

        /// <summary>
        /// Creates a new instance of the <see cref="MeshInterface"/> class.
        /// </summary>
        /// <param name="The maximum number of primitives allowed for this User Interface."></param>
        public MeshInterface(int maxVertices = DefaultSize, int maxIndices = 3 * DefaultSize)
        {
            this.maxVertices = maxVertices;
            this.maxIndices = maxIndices;
            format = Format.R16_UInt;
        }

        public void Initialize(IDirectXProvider directX)
        {
            BufferDescription vbDesc = new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                SizeInBytes = VertexPositionColor.Stride,
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Dynamic
            };

            BufferDescription ibDesc = new BufferDescription
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                SizeInBytes = sizeof(ushort),
                OptionFlags = ResourceOptionFlags.None,
                Usage = ResourceUsage.Dynamic
            };
            
            vertexBuffer = Buffer.Create(directX.Direct3D.Device, vertices, vbDesc);
            indexBuffer = Buffer.Create(directX.Direct3D.Device, indices, ibDesc);

            vertexBuffer.DebugName = "InterfaceMesh_vb";
            indexBuffer.DebugName = "InterfaceMesh_ib";

            vbBinding = new VertexBufferBinding(vertexBuffer, vbDesc.SizeInBytes, 0);
        }

        public void Render(IDirectXTarget target)
        {
            DeviceContext context = target.Direct3D.Context;
            context.InputAssembler.SetIndexBuffer(indexBuffer, format, 0);
            context.InputAssembler.SetVertexBuffers(0, vbBinding);
            context.DrawIndexed(indexCount, 0, 0);
        }

        public void Build(IEnumerable<ShapeMeshDescription> shapes)
        {
            Contract.Ensures(vertices.Length > 0 && indices.Length > 0);
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
            List<ushort> indexList = new List<ushort>();
            ushort index = 0;

            foreach (var shape in shapes)
            {
                vertexList.AddRange(shape.Vertices);
                ushort[] indexArray = shape.Indices;
                for (int i = 0; i < indexArray.Length; i++)
                    indexArray[i] += index;
                indexList.AddRange(indexArray);
                index += (ushort)indexArray.Length;
            }

            vertices = vertexList.ToArray();
            indices = indexList.ToArray();
            indexCount = indices.Length;
        }
    }
}
