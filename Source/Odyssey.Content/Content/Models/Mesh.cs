using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Odyssey.Content.Meshes;
using Odyssey.Engine;
using Odyssey.Geometry;
using Odyssey.Graphics.Meshes;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace Odyssey.Content.Models
{
    public class Mesh : Component, IMesh, IByteSize
    {
        private static int count;
        private readonly MeshData meshData;
        private Buffer indexBuffer;
        private Buffer vertexBuffer;

        public Mesh(string name, PrimitiveTopology pType, VertexDescription vDescription, byte[] vertices, byte[] indices,
            CpuAccessFlags cpuFlags = CpuAccessFlags.None, ResourceUsage rUsage = ResourceUsage.Default)
        {
            Contract.Requires<ArgumentNullException>(vertices != null);
            Contract.Requires<ArgumentException>(vertices.Length > 0);
            Contract.Requires<ArgumentNullException>(indices != null);
            Contract.Requires<ArgumentException>(indices.Length > 0);
            Name = name;
            Id = count++;
            CpuAccessFlags = cpuFlags;
            ResourceUsage = rUsage;
            VertexDescription = vDescription;
            MeshHeader header = new MeshHeader(name, vDescription.Format, vertices.Length, vDescription.Stride,
                Format.R16_UInt, indices.Length, (int)SharpDX.DXGI.FormatHelper.SizeOfInBytes(Format.R16_UInt), pType);

            meshData = new MeshData(header, vertices, indices);
        }

        public Mesh(MeshData meshData, CpuAccessFlags cpuFlags = CpuAccessFlags.None, ResourceUsage rUsage = ResourceUsage.Default)
        {
            Contract.Requires<ArgumentNullException>(meshData != null);
            Name = meshData.Header.Name;
            Id = count++;
            CpuAccessFlags = cpuFlags;
            ResourceUsage = rUsage;
            VertexDescription = new VertexDescription(meshData.Header.VertexFormat, meshData.Header.VertexSize);
            this.meshData = meshData;
        }

        public Mesh(string id, PrimitiveTopology pType, VertexDescription vDescription) :
            this(id, pType, vDescription, null, null)
        { }

        #region Properties

        int IByteSize.ByteSize
        {
            get
            {
                return meshData.Header.ModelByteSize;
            }
        }

        public int Id { get; private set; }

        public MeshHeader Header { get { return meshData.Header; } }

        public Buffer IndexBuffer { get { return indexBuffer; } }

        public bool Inited { get; private set; }

        public Buffer VertexBuffer { get { return vertexBuffer; } }

        public VertexDescription VertexDescription { get; private set; }

        protected CpuAccessFlags CpuAccessFlags { get; private set; }

        protected ResourceUsage ResourceUsage { get; private set; }

        #endregion Properties

        static DataStream CreateDataStream(int length, byte[] data)
        {
            DataStream stream = new DataStream(length, false, true);
            stream.WriteRange(data);
            stream.Position = 0;
            return stream;
        }

        public static void UpdateBuffer<TArray>(IDirectXProvider directX, TArray[] data, Buffer buffer)
             where TArray : struct
        {
            DeviceContext context = directX.Direct3D.Context;

            DataBox box = context.MapSubresource(buffer, 0, MapMode.WriteDiscard, MapFlags.None);
            SharpDX.Utilities.Write(box.DataPointer, data, 0, data.Length);
            context.UnmapSubresource(buffer, 0);
        }

        public TVertex[] AccessVertices<TVertex>()
            where TVertex:struct,IVertex
        {
            return Serializer.DeserializeStructArray<TVertex>(meshData.Vertices, 0, meshData.Header.VertexCount);
        }

        public bool CheckFormat(VertexFormat format)
        {
            return (VertexDescription.Format & format) == format;
        }

        public virtual void Initialize(IDirectXProvider directX)
        {
            RemoveAndDispose(ref vertexBuffer);
            RemoveAndDispose(ref indexBuffer);

            using (DataStream vbStream = CreateDataStream(meshData.Header.VerticesByteSize, meshData.Vertices))
            {
                vertexBuffer = ToDispose(new Buffer(directX.Direct3D.Device, vbStream, new BufferDescription
                {
                    BindFlags = BindFlags.VertexBuffer,
                    CpuAccessFlags = CpuAccessFlags,
                    OptionFlags = ResourceOptionFlags.None,
                    SizeInBytes = meshData.Header.VerticesByteSize,
                    Usage = ResourceUsage,
                }) { DebugName = Name + "_VB" });
            }

            using (DataStream ibStream = CreateDataStream(meshData.Header.IndicesByteSize, meshData.Indices))
            {
                indexBuffer = ToDispose(new Buffer(directX.Direct3D.Device, ibStream, new BufferDescription
                {
                    BindFlags = BindFlags.IndexBuffer,
                    CpuAccessFlags = CpuAccessFlags,
                    OptionFlags = ResourceOptionFlags.None,
                    SizeInBytes = (int)ibStream.Length,
                    Usage = ResourceUsage,
                }) { DebugName = Name + "_IB" });
            }

            Inited = true;
        }

        public void Render(IDirectXProvider directX)
        {
            Render(directX, meshData.Header.IndexCount);
        }

        public virtual void Render(IDirectXProvider directX, int indexCount, int vertexOffset = 0, int indexOffset = 0, int startIndex = 0, int baseVertex = 0)
        {
            DeviceContext context = directX.Direct3D.Context;
            context.InputAssembler.PrimitiveTopology = meshData.Header.PrimitiveTopology;

            context.InputAssembler.SetVertexBuffers(
               0, new VertexBufferBinding(VertexBuffer, VertexDescription.Stride, vertexOffset));
            context.InputAssembler.SetIndexBuffer(IndexBuffer, meshData.Header.IndexFormat, indexOffset);
            context.DrawIndexed(indexCount, startIndex, baseVertex);
        }

        //public void UpdateBuffers(IDirectXProvider directX, IVertex[] vertices, ushort[] indices)
        //{
        //    UpdateBuffer<TVertex>(directX, vertices, VertexBuffer);
        //    UpdateBuffer<ushort>(directX, indices, IndexBuffer);
        //}

        //public void UpdateIndexBuffer(IDirectXProvider directX, ushort[] indices)
        //{
        //    UpdateBuffer<ushort>(directX, indices, IndexBuffer);
        //}

        //public void UpdateVertexBuffer(IDirectXProvider directX, TVertex[] vertices)
        //{
        //    UpdateBuffer<TVertex>(directX, vertices, VertexBuffer);
        //}
    }
}