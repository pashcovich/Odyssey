using Odyssey.Content.Models;
using Odyssey.Graphics.Meshes;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odyssey.Content.Meshes
{
    public class MeshData : IDataSerializable, IByteSize
    {
        private MeshHeader header;
        private byte[] vertices;
        private byte[] indices;

        public Model ParentMesh { get; set; }
        public MeshHeader Header { get { return header; } }
        public byte[] Vertices { get { return vertices; } }
        public byte[] Indices { get { return indices; } }

        public MeshData()
        { }

        public MeshData(MeshHeader header, byte[] vertices, byte[] indices)
        {
            this.header = header;
            this.vertices = vertices;
            this.indices = indices;
        }

        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(ref header);
            serializer.Serialize(ref vertices);
            serializer.Serialize(ref indices);
        }


        public int ByteSize
        {
            get { return vertices.Length + indices.Length; }
        }
    }
}
