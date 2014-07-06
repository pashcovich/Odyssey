using System.Runtime.InteropServices;
using Odyssey.Geometry;
using SharpDX.Direct3D;
using SharpDX.DXGI;
using SharpDX.Serialization;

namespace Odyssey.Graphics.Meshes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public sealed class MeshHeader : IDataSerializable
    {
        private VertexFormat vertexFormat;
        private int vertexCount;
        private int vertexSize;
        private int indexSize;
        private int indexCount;
        private PrimitiveTopology primitiveTopology;
        private VertexElement[] vertexElements;

        #region Properties
        public VertexFormat VertexFormat
        {
            get { return vertexFormat; }
            private set { vertexFormat = value; }
        }

        public int VertexCount
        {
            get { return vertexCount; }
            private set { vertexCount = value; }
        }

        public int VertexSize
        {
            get { return vertexSize; }
            private set { vertexSize = value; }
        }

        public bool IsIndex32Bit
        {
            get { return IndexSize == 4; }
        }

        /// <summary>
        /// Index size, in bytes.
        /// </summary>
        public int IndexSize
        {
            get { return indexSize; }
            private set { indexSize = value; }
        }

        public int IndexCount
        {
            get { return indexCount; }
            private set { indexCount = value; }
        }

        public PrimitiveTopology PrimitiveTopology
        {
            get { return primitiveTopology; }
            private set { primitiveTopology = value; }
        }

        public VertexElement[] VertexElements
        {
            get { return vertexElements; }
            private set { vertexElements = value; }
        }
        #endregion

        public MeshHeader()
        {}

        public MeshHeader(VertexFormat vxFormat, int vxCount, bool isIndex32Bit, int vxSize, int ixCount, PrimitiveTopology primitiveTopology, VertexElement[] vertexElements)
        {
            VertexFormat = vxFormat;
            VertexCount = vxCount;
            VertexSize = vxSize;
            IndexCount = ixCount;
            IndexSize = isIndex32Bit ? 4 : 2;
            PrimitiveTopology = primitiveTopology;
            VertexElements = vertexElements;
        }

        public int VerticesByteSize
        {
            get { return VertexCount * VertexSize; }
        }

        public int IndicesByteSize
        {
            get { return IndexCount * IndexSize; }
        }

        public int ModelByteSize
        {
            get { return Marshal.SizeOf(this) + VerticesByteSize + IndicesByteSize; }
        }

        public int Faces
        {
            get { return IndexCount / 3; }
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.SerializeEnum(ref vertexFormat);
            serializer.Serialize(ref vertexCount);
            serializer.Serialize(ref vertexSize);
            serializer.Serialize(ref indexCount);
            serializer.Serialize(ref indexSize);
            serializer.Serialize(ref vertexElements);
            serializer.SerializeEnum(ref primitiveTopology);
        }
    }
}
